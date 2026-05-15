using Spectre.Console;
using System.Text;

const int boardWidth = 47;
const int boardHeight = 24;
const int paddleWidth = 10;
const int brickRows = 5;
const int brickColumns = 9;
const int brickWidth = 5;
const int frameDelayMs = 16;

var bricks = new List<Brick>();
var bricksRemaining = 0;

var paddleX = 0;
var paddleY = boardHeight - 2;

double ballX = 0;
double ballY = 0;
double velocityX = 0;
double velocityY = 0;

var lost = false;
var won = false;
var score = 0;
var brickBreakAvailable = true;

if (!AnsiConsole.Profile.Capabilities.Interactive)
{
	AnsiConsole.MarkupLine("[red]This game needs an interactive terminal.[/]");
	return;
}

if (!ShowTitlePage())
{
	return;
}

Console.CursorVisible = false;

try
{
	ResetGameState();

	while (true)
	{
		AnsiConsole.Live(new Panel(new Text("Loading..."))
			.Header("Breakout")
			.Border(BoxBorder.Rounded)
			.Expand())
			.Start(context =>
			{
				while (!lost && !won)
				{
					HandleInput(ref paddleX);
					UpdateBall();

					var frame = RenderFrame();

					context.UpdateTarget(new Panel(new Markup(frame))
						.Header("Breakout")
						.Border(BoxBorder.Rounded)
						.Expand());

					Thread.Sleep(frameDelayMs);
				}
			});

		if (won)
		{
			AnsiConsole.MarkupLine($"[green]You win![/] Final score: [yellow]{score}[/]");
			AnsiConsole.MarkupLine("Press any key to exit...");
			Console.ReadKey(true);
			break;
		}

		var choice = ShowGameOverPage();
		if (choice == ConsoleKey.R)
		{
			ResetGameState();
			continue;
		}


	ConsoleKey ShowGameOverPage()
	{
		AnsiConsole.Clear();
		AnsiConsole.Write(new FigletText("Game Over")
			.Color(Color.Red)
			.Centered());

		AnsiConsole.WriteLine();
		AnsiConsole.Write(new Align(new Markup("[red]You missed the ball![/]"), HorizontalAlignment.Center));
		AnsiConsole.WriteLine();
		AnsiConsole.Write(new Align(new Markup($"[yellow]Final score:[/] [white]{score}[/]"), HorizontalAlignment.Center));
		AnsiConsole.WriteLine();
		AnsiConsole.Write(new Align(new Markup($"[yellow]Remaining bricks:[/] [white]{bricksRemaining}[/]"), HorizontalAlignment.Center));
		AnsiConsole.WriteLine();
		AnsiConsole.WriteLine();
		AnsiConsole.Write(new Align(new Markup("[yellow]Press R to restart[/]  [grey]|[/]  [yellow]Press Q to quit[/]"), HorizontalAlignment.Center));

		return ReadRestartChoice();
	}
		break;
	}
}
finally
{
	Console.CursorVisible = true;
}

return;

bool ShowTitlePage()
{
	AnsiConsole.Clear();
	AnsiConsole.Write(new FigletText("Breakout")
		.Color(Color.Red)
		.Centered());

	AnsiConsole.WriteLine();
	AnsiConsole.Write(new Align(new Markup("[white]Use the left and right arrows to move your paddle and keep the ball in play.[/]"), HorizontalAlignment.Center));
	AnsiConsole.WriteLine();
	AnsiConsole.Write(new Align(new Markup("[white]Break all red bricks, then let the ball exit through the top to win.[/]"), HorizontalAlignment.Center));
	AnsiConsole.WriteLine();
	AnsiConsole.Write(new Align(new Markup("[yellow]Press Enter to play[/]  [grey]|[/]  [yellow]Press Q to quit[/]"), HorizontalAlignment.Center));

	while (true)
	{
		var key = Console.ReadKey(true).Key;
		if (key == ConsoleKey.Enter)
		{
			AnsiConsole.Clear();
			return true;
		}

		if (key == ConsoleKey.Q)
		{
			return false;
		}
	}
}

void ResetGameState()
{
	while (Console.KeyAvailable)
	{
		Console.ReadKey(true);
	}

	bricks = BuildBricks();
	bricksRemaining = bricks.Count;
	paddleX = (boardWidth - paddleWidth) / 2;
	ballX = boardWidth / 2.0;
	ballY = boardHeight - 5.0;
	velocityX = 0.35;
	velocityY = -0.35;
	lost = false;
	won = false;
	score = 0;
	brickBreakAvailable = true;
}

ConsoleKey ReadRestartChoice()
{
	while (true)
	{
		var key = Console.ReadKey(true).Key;
		if (key is ConsoleKey.R or ConsoleKey.Q)
		{
			return key;
		}
	}
}

List<Brick> BuildBricks()
{
	var list = new List<Brick>();
	var totalBrickWidth = brickColumns * brickWidth;
	var startX = (boardWidth - totalBrickWidth) / 2;

	for (var row = 0; row < brickRows; row++)
	{
		for (var col = 0; col < brickColumns; col++)
		{
			list.Add(new Brick(startX + (col * brickWidth), 1 + row));
		}
	}

	return list;
}

void HandleInput(ref int currentPaddleX)
{
	while (Console.KeyAvailable)
	{
		var key = Console.ReadKey(true).Key;

		if (key == ConsoleKey.LeftArrow)
		{
			currentPaddleX = Math.Max(0, currentPaddleX - 2);
		}
		else if (key == ConsoleKey.RightArrow)
		{
			currentPaddleX = Math.Min(boardWidth - paddleWidth, currentPaddleX + 2);
		}
	}
}

void UpdateBall()
{
	var nextX = ballX + velocityX;
	var nextY = ballY + velocityY;

	if (nextX < 0)
	{
		nextX = 0;
		velocityX *= -1;
	}
	else if (nextX > boardWidth - 1)
	{
		nextX = boardWidth - 1;
		velocityX *= -1;
	}

	if (nextY < 0)
	{
		if (bricksRemaining == 0)
		{
			won = true;
			return;
		}

		nextY = 0;
		velocityY *= -1;
	}

	if (velocityY > 0 && nextY >= paddleY && ballY <= paddleY + 0.5)
	{
		var paddleHitX = (int)Math.Round(nextX);
		if (paddleHitX >= paddleX && paddleHitX <= paddleX + paddleWidth - 1)
		{
			// Offset controls rebound angle so center hits are straighter.
			var paddleCenter = paddleX + (paddleWidth / 2.0);
			var offset = (nextX - paddleCenter) / (paddleWidth / 2.0);

			velocityX = Clamp(offset * 0.55, -0.75, 0.75);
			velocityY = -Math.Abs(velocityY);
			nextY = paddleY - 0.01;
			brickBreakAvailable = true;
		}
	}

	var struckBrick = false;
	if (brickBreakAvailable)
	{
		for (var i = 0; i < bricks.Count; i++)
		{
			if (!bricks[i].Active)
			{
				continue;
			}

			var brick = bricks[i];
			if (nextX >= brick.X && nextX < brick.X + brickWidth && nextY >= brick.Y && nextY < brick.Y + 1)
			{
				bricks[i] = brick with { Active = false };
				bricksRemaining--;
				score += 100;
				brickBreakAvailable = false;
				struckBrick = true;
				break;
			}
		}
	}

	if (struckBrick)
	{
		velocityY *= -1;
		nextY = ballY + velocityY;
	}

	if (nextY > boardHeight - 1)
	{
		lost = true;
		return;
	}

	ballX = nextX;
	ballY = nextY;
}

string RenderFrame()
{
	var board = new char[boardHeight, boardWidth];
	for (var y = 0; y < boardHeight; y++)
	{
		for (var x = 0; x < boardWidth; x++)
		{
			board[y, x] = ' ';
		}
	}

	for (var y = 0; y < boardHeight; y++)
	{
		board[y, 0] = '|';
		board[y, boardWidth - 1] = '|';
	}

	for (var x = 0; x < boardWidth; x++)
	{
		board[boardHeight - 1, x] = '-';
	}

	foreach (var brick in bricks)
	{
		if (!brick.Active)
		{
			continue;
		}

		for (var w = 0; w < brickWidth; w++)
		{
			var bx = brick.X + w;
			if (bx > 0 && bx < boardWidth - 1)
			{
				board[brick.Y, bx] = '=';
			}
		}
	}

	for (var w = 0; w < paddleWidth; w++)
	{
		var px = paddleX + w;
		if (px > 0 && px < boardWidth - 1)
		{
			board[paddleY, px] = '_';
		}
	}

	var roundedBallX = (int)Math.Round(ballX);
	var roundedBallY = (int)Math.Round(ballY);
	if (roundedBallX > 0 && roundedBallX < boardWidth - 1 && roundedBallY >= 0 && roundedBallY < boardHeight)
	{
		board[roundedBallY, roundedBallX] = 'O';
	}

	var sb = new StringBuilder();
	sb.AppendLine($"Score: {score}   Bricks: {bricksRemaining}   Controls: Left/Right arrows");

	for (var y = 0; y < boardHeight; y++)
	{
		var hasBallOnRow = false;
		for (var checkX = 0; checkX < boardWidth; checkX++)
		{
			if (board[y, checkX] == 'O')
			{
				hasBallOnRow = true;
				break;
			}
		}

		var compensationX = -1;
		if (hasBallOnRow)
		{
			compensationX = boardWidth - 2;
			if (board[y, compensationX] == 'O')
			{
				compensationX = boardWidth - 3;
			}
		}

		for (var x = 0; x < boardWidth; x++)
		{
			if (x == compensationX)
			{
				continue;
			}

			if (board[y, x] == '=')
			{
				sb.Append("[red]=[/]");
			}
			else if (board[y, x] == '_')
			{
				sb.Append("[purple]_[/]");
			}
			else if (board[y, x] == 'O')
			{
				sb.Append(velocityY < 0 ? "😄" : "😲");
			}
			else
			{
				sb.Append(board[y, x]);
			}
		}

		if (y < boardHeight - 1)
		{
			sb.AppendLine();
		}
	}

	return sb.ToString();
}

static double Clamp(double value, double min, double max)
	=> Math.Min(max, Math.Max(min, value));

record struct Brick(int X, int Y, bool Active = true);
