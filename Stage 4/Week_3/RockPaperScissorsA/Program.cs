var random = new Random();
var playerWins = 0;
var computerWins = 0;
var ties = 0;
var bestOf = PromptForBestOf();
var winsNeeded = bestOf / 2 + 1;

ShowWelcome(bestOf, winsNeeded);

while (playerWins < winsNeeded && computerWins < winsNeeded)
{
    Console.Write("Choose rock, paper, or scissors (or q to quit): ");
    var input = Console.ReadLine()?.Trim().ToLowerInvariant();

    if (string.IsNullOrWhiteSpace(input))
    {
        WriteColoredLine("Please enter a choice.\n", ConsoleColor.Yellow);
        continue;
    }

    if (input is "q" or "quit" or "exit")
    {
        break;
    }

    if (!TryParseMove(input, out var playerMove))
    {
        WriteColoredLine("Invalid choice. Try rock, paper, scissors, r, p, or s.\n", ConsoleColor.Yellow);
        continue;
    }

    var computerMove = (Move)random.Next(0, 3);
    var result = DetermineWinner(playerMove, computerMove);

    WriteColoredLine($"You chose {playerMove}.", ConsoleColor.Cyan);
    WriteColoredLine($"Computer chose {computerMove}.", ConsoleColor.DarkCyan);

    switch (result)
    {
        case 0:
            ties++;
            WriteColoredLine("It's a tie!", ConsoleColor.Yellow);
            break;
        case 1:
            playerWins++;
            WriteColoredLine("You win!", ConsoleColor.Green);
            break;
        default:
            computerWins++;
            WriteColoredLine("Computer wins!", ConsoleColor.Red);
            break;
    }

    WriteColoredLine($"Score: You {playerWins} | Computer {computerWins} | Ties {ties}\n", ConsoleColor.White);
}

Console.WriteLine();
if (playerWins == winsNeeded)
{
    WriteColoredLine($"You won the best-of-{bestOf} match!", ConsoleColor.Green);
}
else if (computerWins == winsNeeded)
{
    WriteColoredLine($"Computer won the best-of-{bestOf} match.", ConsoleColor.Red);
}
else
{
    WriteColoredLine("Match ended early.", ConsoleColor.Yellow);
}

WriteColoredLine("Final score:", ConsoleColor.White);
WriteColoredLine($"You: {playerWins}", ConsoleColor.Cyan);
WriteColoredLine($"Computer: {computerWins}", ConsoleColor.DarkCyan);
WriteColoredLine($"Ties: {ties}", ConsoleColor.Yellow);
WriteColoredLine("Thanks for playing.", ConsoleColor.White);

static void ShowWelcome(int bestOf, int winsNeeded)
{
    WriteColoredLine("Rock Paper Scissors", ConsoleColor.Magenta);
    WriteColoredLine("-------------------", ConsoleColor.Magenta);
    Console.WriteLine("Enter rock, paper, or scissors to play.");
    Console.WriteLine("Shortcuts: r, p, s");
    Console.WriteLine($"Match type: Best of {bestOf} ({winsNeeded} wins needed)");
    Console.WriteLine("Type q to quit.\n");
}

static int PromptForBestOf()
{
    while (true)
    {
        Console.Write("Choose match length: best of 3 or 5? ");
        var input = Console.ReadLine()?.Trim();

        if (input == "3" || input == "5")
        {
            return int.Parse(input);
        }

        WriteColoredLine("Please enter 3 or 5.\n", ConsoleColor.Yellow);
    }
}

static void WriteColoredLine(string message, ConsoleColor color)
{
    var originalColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ForegroundColor = originalColor;
}

static bool TryParseMove(string input, out Move move)
{
    switch (input)
    {
        case "rock":
        case "r":
            move = Move.Rock;
            return true;
        case "paper":
        case "p":
            move = Move.Paper;
            return true;
        case "scissors":
        case "s":
            move = Move.Scissors;
            return true;
        default:
            move = default;
            return false;
    }
}

static int DetermineWinner(Move playerMove, Move computerMove)
{
    if (playerMove == computerMove)
    {
        return 0;
    }

    return (playerMove, computerMove) switch
    {
        (Move.Rock, Move.Scissors) => 1,
        (Move.Paper, Move.Rock) => 1,
        (Move.Scissors, Move.Paper) => 1,
        _ => -1
    };
}

enum Move
{
    Rock,
    Paper,
    Scissors
}