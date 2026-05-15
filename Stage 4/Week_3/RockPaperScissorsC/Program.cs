namespace RockPaperScissorsC;

internal enum Choice
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

internal enum RoundOutcome
{
    Win,
    Lose,
    Draw
}

internal static class Program
{
    private static readonly Random Random = new();

    private static int _wins;
    private static int _losses;
    private static int _draws;

    private static void Main()
    {
        PrintWelcome();

        bool playAgain;
        do
        {
            Choice playerChoice = GetPlayerChoice();
            Choice computerChoice = GetComputerChoice();

            RoundOutcome outcome = DetermineRoundOutcome(playerChoice, computerChoice);
            UpdateScore(outcome);
            PrintRoundResult(playerChoice, computerChoice, outcome);

            playAgain = AskToPlayAgain();
            Console.WriteLine();
        }
        while (playAgain);

        PrintFinalScore();
    }

    private static void PrintWelcome()
    {
        Console.WriteLine("=== Rock Paper Scissors ===");
        Console.WriteLine("Choose an option each round:");
        Console.WriteLine("1 = Rock, 2 = Paper, 3 = Scissors");
        Console.WriteLine();
    }

    private static Choice GetPlayerChoice()
    {
        while (true)
        {
            Console.Write("Enter your choice (1-3): ");
            string? input = Console.ReadLine();

            // Validate that input is an integer and maps to a valid enum value.
            if (int.TryParse(input, out int numericChoice) &&
                Enum.IsDefined(typeof(Choice), numericChoice))
            {
                return (Choice)numericChoice;
            }

            Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
        }
    }

    private static Choice GetComputerChoice()
    {
        return (Choice)Random.Next(1, 4);
    }

    private static RoundOutcome DetermineRoundOutcome(Choice player, Choice computer)
    {
        if (player == computer)
        {
            return RoundOutcome.Draw;
        }

        bool playerWins =
            (player == Choice.Rock && computer == Choice.Scissors) ||
            (player == Choice.Paper && computer == Choice.Rock) ||
            (player == Choice.Scissors && computer == Choice.Paper);

        return playerWins ? RoundOutcome.Win : RoundOutcome.Lose;
    }

    private static void UpdateScore(RoundOutcome outcome)
    {
        switch (outcome)
        {
            case RoundOutcome.Win:
                _wins++;
                break;
            case RoundOutcome.Lose:
                _losses++;
                break;
            default:
                _draws++;
                break;
        }
    }

    private static void PrintRoundResult(Choice player, Choice computer, RoundOutcome outcome)
    {
        Console.WriteLine($"You chose: {player}");
        Console.WriteLine($"Computer chose: {computer}");

        string message = outcome switch
        {
            RoundOutcome.Win => "You win this round!",
            RoundOutcome.Lose => "You lose this round.",
            _ => "This round is a draw."
        };

        Console.WriteLine(message);
        Console.WriteLine($"Score -> Wins: {_wins}, Losses: {_losses}, Draws: {_draws}");
    }

    private static bool AskToPlayAgain()
    {
        while (true)
        {
            Console.Write("Play another round? (y/n): ");
            string? input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (input == "y" || input == "yes")
            {
                return true;
            }

            if (input == "n" || input == "no")
            {
                return false;
            }

            Console.WriteLine("Invalid input. Please type 'y' or 'n'.");
        }
    }

    private static void PrintFinalScore()
    {
        Console.WriteLine("=== Final Score ===");
        Console.WriteLine($"Wins: {_wins}");
        Console.WriteLine($"Losses: {_losses}");
        Console.WriteLine($"Draws: {_draws}");
        Console.WriteLine("Thanks for playing!");
    }
}
