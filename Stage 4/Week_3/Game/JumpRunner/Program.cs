using Spectre.Console;
using System.Diagnostics;
using System.Text;

var game = new JumpRunnerGame();
game.Run();

internal sealed class JumpRunnerGame
{
	private const int TrackWidth = 44;
	private const int WorldHeight = 14;
	private const int GroundRow = 10;
	private const int GroundThickness = 2;
	private const int PlayerColumn = 8;
	private const int JumpVelocity = -4;
	private const int Gravity = 1;
	private const int BaseFrameMs = 70;
	private const int MinimumFrameMs = 38;
	private const int SpeedUpEveryScore = 80;

	private readonly Random _random = new();
	private readonly List<int> _ground = [];

	private int _playerRow;
	private int _verticalVelocity;
	private bool _isGameOver;
	private double _score;
	private int _framesUntilNextHole;
	private int _remainingHoleTiles;

	public void Run()
	{
		Console.OutputEncoding = Encoding.UTF8;
		AnsiConsole.Clear();

		if (!ShowTitleScreen())
		{
			AnsiConsole.Clear();
			AnsiConsole.MarkupLine("[grey]Thanks for playing.[/]");
			return;
		}

		while (true)
		{
			InitializeRun();
			PlayRound();

			var shouldRestart = ShowGameOver();
			if (!shouldRestart)
			{
				break;
			}
		}

		AnsiConsole.Clear();
		AnsiConsole.MarkupLine("[grey]Thanks for playing.[/]");
	}

	private bool ShowTitleScreen()
	{
		AnsiConsole.Clear();

		var title = new FigletText("Jump Runner")
			.Centered()
			.Color(Color.Aqua);

		AnsiConsole.Write(title);

		var intro = new Panel(
			Align.Center(
				new Markup(
					"[white]Auto-run through a shifting path and jump over holes.[/]\n" +
					"[white]Survive longer to push your score higher.[/]\n\n" +
					"[grey]Press[/] [bold]ENTER[/] [grey]to start or[/] [bold]Q[/] [grey]to quit.[/]")))
			.Border(BoxBorder.Rounded)
			.BorderStyle(new Style(Color.Grey))
			.Header(new PanelHeader("[bold yellow]How To Play[/]", Justify.Center));
		intro.Expand = false;

		AnsiConsole.Write(Align.Center(intro));

		while (true)
		{
			var key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Enter)
			{
				return true;
			}

			if (key == ConsoleKey.Q || key == ConsoleKey.Escape)
			{
				return false;
			}
		}
	}

	private void InitializeRun()
	{
		_ground.Clear();
		for (var index = 0; index < TrackWidth; index++)
		{
			_ground.Add(1);
		}

		_playerRow = GroundRow - 1;
		_verticalVelocity = 0;
		_isGameOver = false;
		_score = 0;
		_framesUntilNextHole = 10;
		_remainingHoleTiles = 0;

		while (Console.KeyAvailable)
		{
			Console.ReadKey(true);
		}
	}

	private void PlayRound()
	{
		var watch = Stopwatch.StartNew();
		AnsiConsole.AlternateScreen(() =>
		{
			AnsiConsole.Live(CreateGamePanel())
				.AutoClear(false)
				.Overflow(VerticalOverflow.Crop)
				.Cropping(VerticalOverflowCropping.Top)
				.Start(context =>
				{
					var lastFrameAt = watch.ElapsedMilliseconds;

					while (!_isGameOver)
					{
						HandleInput();
						UpdateWorld();

						context.UpdateTarget(CreateGamePanel());
						context.Refresh();

						var now = watch.ElapsedMilliseconds;
						var delay = GetCurrentFrameDurationMs() - (int)(now - lastFrameAt);
						lastFrameAt = now;

						if (delay > 0)
						{
							Thread.Sleep(delay);
						}
					}
				});
		});
	}

	private int GetCurrentFrameDurationMs()
	{
		var speedIncreaseSteps = (int)(_score / SpeedUpEveryScore);
		var currentFrameMs = BaseFrameMs - speedIncreaseSteps;
		return Math.Max(MinimumFrameMs, currentFrameMs);
	}

	private Panel CreateGamePanel()
	{
		return new Panel(new Markup(RenderFrame()))
			.Header($"[bold yellow]Jump Runner[/]   [white]Score:[/] [bold aqua]{Math.Floor(_score)}[/]")
			.Border(BoxBorder.Rounded)
			.BorderStyle(new Style(Color.Green));
	}

	private void HandleInput()
	{
		while (Console.KeyAvailable)
		{
			var key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Spacebar && IsPlayerGrounded())
			{
				_verticalVelocity = JumpVelocity;
			}
		}
	}

	private void UpdateWorld()
	{
		AdvanceGround();

		_playerRow += _verticalVelocity;
		_verticalVelocity += Gravity;

		if (IsPlayerInHole())
		{
			_isGameOver = true;
			return;
		}

		var surfaceRow = GetGroundSurfaceRow(PlayerColumn);
		if (_playerRow >= surfaceRow)
		{
			_playerRow = surfaceRow;
			_verticalVelocity = 0;
		}

		if (_playerRow > WorldHeight - 1)
		{
			_isGameOver = true;
		}

		_score += 1.35;
	}

	private void AdvanceGround()
	{
		_ground.RemoveAt(0);

		if (_remainingHoleTiles > 0)
		{
			_ground.Add(0);
			_remainingHoleTiles--;
			if (_remainingHoleTiles == 0)
			{
				_framesUntilNextHole = _random.Next(8, 14);
			}
		}
		else if (_framesUntilNextHole <= 0)
		{
			_ground.Add(0);
			_remainingHoleTiles = _random.Next(1, 3);
		}
		else
		{
			_ground.Add(1);
			_framesUntilNextHole--;
		}
	}

	private bool IsPlayerGrounded()
	{
		return _playerRow == GetGroundSurfaceRow(PlayerColumn) && _ground[PlayerColumn] == 1;
	}

	private bool IsPlayerInHole()
	{
		return _ground[PlayerColumn] == 0 && _playerRow >= GroundRow;
	}

	private int GetGroundSurfaceRow(int column)
	{
		return _ground[column] == 1 ? GroundRow - 1 : WorldHeight;
	}

	private string RenderFrame()
	{
		var builder = new StringBuilder();

		builder.AppendLine("[grey]Press [bold]SPACE[/] to jump over holes. Survive as long as you can.[/]");
		builder.AppendLine();

		for (var row = 0; row < WorldHeight; row++)
		{
			for (var column = 0; column < TrackWidth; column++)
			{
				if (row == _playerRow && column == PlayerColumn)
				{
					builder.Append("[bold deepskyblue1]@[/]");
					continue;
				}

				if (row >= GroundRow && row < GroundRow + GroundThickness && _ground[column] == 1)
				{
					builder.Append("[green3]=[/]");
				}
				else if (row == GroundRow - 1 && _ground[column] == 1)
				{
					builder.Append("[chartreuse1]_[/]");
				}
				else if (row >= GroundRow)
				{
					builder.Append(" ");
				}
				else
				{
					builder.Append("[grey11].[/]");
				}
			}

			builder.AppendLine();
		}

		return builder.ToString();
	}

	private bool ShowGameOver()
	{
		AnsiConsole.Clear();

		var score = (int)Math.Floor(_score);
		var panel = new Panel(
			new Markup(
				$"[bold red]Game Over[/]\n\n" +
				$"[white]Final Score:[/] [bold aqua]{score}[/]\n\n" +
				"[grey]Press[/] [bold]R[/] [grey]to restart or[/] [bold]Q[/] [grey]to quit.[/]"))
			.Border(BoxBorder.Double)
			.Header("[bold]Run Ended[/]")
			.BorderStyle(new Style(Color.Red));

		AnsiConsole.Write(panel);

		while (true)
		{
			var key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.R)
			{
				return true;
			}

			if (key == ConsoleKey.Q || key == ConsoleKey.Escape)
			{
				return false;
			}
		}
	}
}
