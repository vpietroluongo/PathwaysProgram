internal enum Move
{
    Rock,
    Paper,
    Scissors
}

internal static class Program
{
    private static void Main()
    {
        var random = new Random();
        var playerScore = 0;
        var computerScore = 0;
        var ties = 0;

        Console.WriteLine("Rock Paper Scissors");
        Console.WriteLine("Type rock, paper, or scissors to play. Type q to quit.");

        while (true)
        {
            Console.WriteLine();
            Console.Write("Your choice: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please enter rock, paper, scissors, or q.");
                continue;
            }

            input = input.Trim();

            if (input.Equals("q", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (!TryParseMove(input, out var playerMove))
            {
                Console.WriteLine("Invalid choice. Enter rock, paper, scissors, or q.");
                continue;
            }

            var computerMove = (Move)random.Next(0, 3);
            Console.WriteLine($"Computer chose: {computerMove}");

            var result = GetRoundResult(playerMove, computerMove);

            switch (result)
            {
                case 1:
                    playerScore++;
                    Console.WriteLine("You win this round.");
                    break;
                case -1:
                    computerScore++;
                    Console.WriteLine("Computer wins this round.");
                    break;
                default:
                    ties++;
                    Console.WriteLine("This round is a tie.");
                    break;
            }

            Console.WriteLine($"Score -> You: {playerScore} | Computer: {computerScore} | Ties: {ties}");
        }

        Console.WriteLine();
        Console.WriteLine("Final score");
        Console.WriteLine($"You: {playerScore}");
        Console.WriteLine($"Computer: {computerScore}");
        Console.WriteLine($"Ties: {ties}");
    }

    private static bool TryParseMove(string input, out Move move)
    {
        switch (input.Trim().ToLowerInvariant())
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

    private static int GetRoundResult(Move playerMove, Move computerMove)
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
}