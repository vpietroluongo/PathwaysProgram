using Spectre.Console;
using Spectre.Console.Rendering;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "My Awesome Console App";

        AnsiConsole.Write(
            new FigletText("Warm Up")
                .LeftJustified()
                .Color(Color.Yellow));
        // TODO: Build your dashboard here!

        AnsiConsole.MarkupLine("[bold green]Welcome to the Warm-Up Activity![/]");
        AnsiConsole.Write(new Markup("[bold purple]Welcome to the Warm-Up Activity using Write new markup![/]"));
        //AnsiConsole.WriteLine(new Text("Welcome using WriteLine new text!", new Style(Color.Red, decoration: Decoration.Bold)));
        AnsiConsole.Write(new Text("Welcome!", new Style(Color.SkyBlue3, decoration: Decoration.Bold)));
        AnsiConsole.WriteLine();
        // Your code goes here...
        AnsiConsole.MarkupLine("[bold red]Hello[/] [green]World[/]!"); 

        //prompt menu
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices("Play Game", "View Stats", "Settings", "Exit"));
        
        switch (choice)
        {
            case "View Stats":
                //progress bar
                AnsiConsole.Progress()
                    .Start(ctx =>
                    {
                        var task = ctx.AddTask("Loading awesome stuff...");
                        while (!ctx.IsFinished)
                        {
                            task.Increment(5);
                            Thread.Sleep(100);
                        }
                    });
                //table
                var table = new Table();
                table.AddColumn("Name");
                table.AddColumn("Score");
                table.AddRow("Alice 🥇", "[green]2450[/]");
                table.AddRow("Bob 🥈", "[yellow]1890[/]");
                table.AddRow("Charlie 😖", "[blue]300[/]");
                AnsiConsole.Write(table);
                break;
            case "Exit":
                AnsiConsole.MarkupLine("[bold purple]Goodbye![/]");
                break;
        }
        
        // AnsiConsole.Progress()
        //     .Start(ctx =>
        //     {
        //         var runFrames = new[] { "🏃    ", " 🏃   ", "  🏃  ", "   🏃 ", "    🏃" };
        //         var frameIndex = 0;
        //         var task = ctx.AddTask($"{runFrames[frameIndex]} Loading awesome stuff...");
        //         while (!ctx.IsFinished)
        //         {
        //             task.Increment(5);
        //             frameIndex = (frameIndex + 1) % runFrames.Length;
        //             task.Description = $"{runFrames[frameIndex]} Loading awesome stuff...";
        //             Thread.Sleep(100);
        //         }
        //     });

    }
}
