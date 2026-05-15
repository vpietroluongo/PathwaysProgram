using System.Diagnostics;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

var game = new PacmanGame();
game.Run();

internal sealed class PacmanGame
{
	private readonly char[][] _maze;
	private readonly HashSet<Position> _strawberries;
	private readonly List<Monster> _monsters;
	private readonly Random _random = new();
	private Position _player;
	private GameState _state = GameState.Running;

	public PacmanGame()
	{
		var layout = new[]
		{
			"###############",
			"#P....#...M..##",
			"#.###.#.###..##",
			"#.....#...#...#",
			"#.#####.#.#.#.#",
			"#...#...#.#.#.#",
			"###.#.###.#.#.#",
			"#...#.....#...#",
			"#.###.#####.#.#",
			"#..M....#....##",
			"###############",
		};

		_maze = new char[layout.Length][];
		_strawberries = new HashSet<Position>();
		_monsters = new List<Monster>();

		for (var row = 0; row < layout.Length; row++)
		{
			_maze[row] = layout[row].ToCharArray();

			for (var column = 0; column < _maze[row].Length; column++)
			{
				var position = new Position(row, column);
				switch (_maze[row][column])
				{
					case 'P':
						_player = position;
						_maze[row][column] = ' ';
						break;
					case 'M':
						_monsters.Add(new Monster(position));
						_maze[row][column] = ' ';
						break;
					case '.':
						_strawberries.Add(position);
						_maze[row][column] = ' ';
						break;
				}
			}
		}

		CollectStrawberry();
	}

	public void Run()
	{
		Console.CursorVisible = false;
		var monsterTimer = Stopwatch.StartNew();

		AnsiConsole.Live(BuildScene())
			.AutoClear(false)
			.Overflow(VerticalOverflow.Visible)
			.Start(context =>
			{
				while (_state == GameState.Running)
				{
					if (Console.KeyAvailable)
					{
						var key = Console.ReadKey(intercept: true).Key;
						HandleInput(key);
					}

					if (_state == GameState.Running && monsterTimer.ElapsedMilliseconds >= 250)
					{
						MoveMonsters();
						monsterTimer.Restart();
					}

					context.UpdateTarget(BuildScene());
					Thread.Sleep(16);
				}

				context.UpdateTarget(BuildScene());
			});

		Console.CursorVisible = true;
		Console.ReadKey(intercept: true);
	}

	private void HandleInput(ConsoleKey key)
	{
		if (key == ConsoleKey.Escape)
		{
			_state = GameState.Quit;
			return;
		}

		var movement = key switch
		{
			ConsoleKey.UpArrow => new Position(-1, 0),
			ConsoleKey.DownArrow => new Position(1, 0),
			ConsoleKey.LeftArrow => new Position(0, -1),
			ConsoleKey.RightArrow => new Position(0, 1),
			_ => new Position(0, 0),
		};

		if (movement == new Position(0, 0))
		{
			return;
		}

		var next = _player + movement;
		if (IsWall(next))
		{
			return;
		}

		_player = next;
		EvaluateCollisions();
		CollectStrawberry();
	}

	private void MoveMonsters()
	{
		for (var index = 0; index < _monsters.Count; index++)
		{
			var monster = _monsters[index];
			var options = Directions
				.Select(direction => monster.Position + direction)
				.Where(position => !IsWall(position))
				.ToList();

			if (options.Count == 0)
			{
				continue;
			}

			Position chosen;
			if (_random.NextDouble() < 0.6)
			{
				chosen = options
					.OrderBy(position => DistanceToPlayer(position))
					.First();
			}
			else
			{
				chosen = options[_random.Next(options.Count)];
			}

			_monsters[index] = monster with { Position = chosen };
		}

		EvaluateCollisions();
	}

	private void CollectStrawberry()
	{
		_strawberries.Remove(_player);
		if (_strawberries.Count == 0)
		{
			_state = GameState.Won;
		}
	}

	private void EvaluateCollisions()
	{
		if (_monsters.Any(monster => monster.Position == _player))
		{
			_state = GameState.Lost;
		}
	}

	private bool IsWall(Position position)
	{
		return position.Row < 0
			|| position.Row >= _maze.Length
			|| position.Column < 0
			|| position.Column >= _maze[0].Length
			|| _maze[position.Row][position.Column] == '#';
	}

	private int DistanceToPlayer(Position position)
	{
		return Math.Abs(position.Row - _player.Row) + Math.Abs(position.Column - _player.Column);
	}

	private IRenderable BuildScene()
	{
		var board = new StringBuilder();
		for (var row = 0; row < _maze.Length; row++)
		{
			for (var column = 0; column < _maze[row].Length; column++)
			{
				var position = new Position(row, column);
				board.Append(GetCellMarkup(position));
			}

			if (row < _maze.Length - 1)
			{
				board.AppendLine();
			}
		}

		var summary = _state switch
		{
			GameState.Running => $"[white]Strawberries left:[/] [yellow]{_strawberries.Count}[/]  [white]Move:[/] [aqua]Arrow keys[/]  [white]Quit:[/] [grey]Esc[/]",
			GameState.Won => "[green]You collected every strawberry. You win![/]\n[white]Press any key to exit.[/]",
			GameState.Lost => "[red]A monster caught you. Game over.[/]\n[white]Press any key to exit.[/]",
			GameState.Quit => "[grey]Game cancelled.[/]\n[white]Press any key to exit.[/]",
			_ => string.Empty,
		};

		return new Rows(
			new Rule("[red]Strawberry Maze[/]") { Justification = Justify.Left },
			new Panel(new Markup(board.ToString()))
			{
				Border = BoxBorder.Rounded,
				Header = new PanelHeader("Collect all strawberries and avoid the monsters"),
			},
			new Markup(summary));
	}

	private string GetCellMarkup(Position position)
	{
		if (_player == position)
		{
			return "[deepskyblue1]🐭[/]";
		}

		if (_monsters.Any(monster => monster.Position == position))
		{
			return "[red]🐱[/]";
		}

		if (_strawberries.Contains(position))
		{
			return "[yellow]🧀[/]";
		}

		return _maze[position.Row][position.Column] switch
		{
			'#' => GetWallMarkup(position),
			_ => " ",
		};
	}

	private string GetWallMarkup(Position position)
	{
		var hasLeft = position.Column > 0 && _maze[position.Row][position.Column - 1] == '#';
		var hasRight = position.Column < _maze[position.Row].Length - 1 && _maze[position.Row][position.Column + 1] == '#';
		var hasUp = position.Row > 0 && _maze[position.Row - 1][position.Column] == '#';
		var hasDown = position.Row < _maze.Length - 1 && _maze[position.Row + 1][position.Column] == '#';

		if ((hasLeft || hasRight) && !(hasUp || hasDown))
		{
			return "[blue]__[/]";
		}

		if (hasUp || hasDown)
		{
			return "[blue]|[/]";
		}

		return "[blue]__[/]";
	}

	private static readonly Position[] Directions =
	{
		new(-1, 0),
		new(1, 0),
		new(0, -1),
		new(0, 1),
	};
}

internal enum GameState
{
	Running,
	Won,
	Lost,
	Quit,
}

internal readonly record struct Position(int Row, int Column)
{
	public static Position operator +(Position left, Position right)
	{
		return new Position(left.Row + right.Row, left.Column + right.Column);
	}
}

internal readonly record struct Monster(Position Position);
