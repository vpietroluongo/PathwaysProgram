using Spectre.Console;
using System.Text;

const int laneCount = 5;
const float trackLength = 120f;
const float baseSpeed = 0.45f;
const float minSpeed = 0.20f;
const float maxSpeed = 1.25f;
const int frameDelayMs = 80;
const int renderWidth = 72;

var random = new Random();
var racers = new List<Racer>
{
	new("Player", 'P', 2, baseSpeed),
	new("CPU 1", '1', 1, baseSpeed),
	new("CPU 2", '2', 3, baseSpeed),
};

var trackItems = new List<TrackItem>();
int tick = 0;
int nextFinishPlace = 1;
string? firstFinisher = null;

AnsiConsole.Clear();
AnsiConsole.MarkupLine("[bold yellow]ASCII Racing[/]");
AnsiConsole.MarkupLine("Use [bold]Left[/]/[bold]Right[/] arrow keys to switch lanes. Reach the finish first.");
AnsiConsole.MarkupLine("Coins increase speed. Obstacles and racer bumps reduce speed.");
Thread.Sleep(1500);

AnsiConsole.Live(new Panel("Starting race...").Header("Race View"))
	.Start(ctx =>
	{
		while (true)
		{
			tick++;

			HandlePlayerInput(racers[0]);
			UpdateAiLanes(racers, trackItems, random);
			SpawnTrackItems(trackItems, racers, random);
			MoveRacers(racers);
			ResolveItemCollisions(racers, trackItems);
			ResolveRacerCollisions(racers);
			ResolveFinishers(racers, ref nextFinishPlace, tick, ref firstFinisher);
			ApplySpeedNormalization(racers);
			DecrementCooldowns(racers);

			var frame = RenderFrame(racers, trackItems, tick, firstFinisher);
			ctx.UpdateTarget(new Panel(new Text(frame)).Header("Race View").Border(BoxBorder.Rounded));

			var playerFinished = racers[0].FinishPlace.HasValue;
			var allFinished = racers.All(r => r.FinishPlace.HasValue);
			if (playerFinished && allFinished)
			{
				break;
			}

			Thread.Sleep(frameDelayMs);
		}
	});

var standings = racers
	.OrderBy(r => r.FinishPlace)
	.ToList();

AnsiConsole.Clear();
AnsiConsole.MarkupLine("[bold green]Race complete![/]");

var table = new Table().Border(TableBorder.Rounded);
table.AddColumn("Place");
table.AddColumn("Racer");
table.AddColumn("Final Speed");
table.AddColumn("Finish Tick");

foreach (var racer in standings)
{
	table.AddRow(
		racer.FinishPlace?.ToString() ?? "-",
		racer.Name,
		racer.Speed.ToString("0.00"),
		racer.FinishTick?.ToString() ?? "-"
	);
}

AnsiConsole.Write(table);

if (standings.Count > 0)
{
	AnsiConsole.MarkupLine($"[bold yellow]Winner:[/] {standings[0].Name}");
}

AnsiConsole.MarkupLine("Press any key to exit...");
Console.ReadKey(true);

static void HandlePlayerInput(Racer player)
{
	if (player.FinishPlace.HasValue)
	{
		return;
	}

	while (Console.KeyAvailable)
	{
		var key = Console.ReadKey(true).Key;
		if (key == ConsoleKey.LeftArrow)
		{
			player.Lane = Math.Max(0, player.Lane - 1);
		}
		else if (key == ConsoleKey.RightArrow)
		{
			player.Lane = Math.Min(laneCount - 1, player.Lane + 1);
		}
	}
}

static void UpdateAiLanes(List<Racer> racers, List<TrackItem> items, Random random)
{
	foreach (var racer in racers.Skip(1))
	{
		if (racer.FinishPlace.HasValue)
		{
			continue;
		}

		var laneScores = new float[laneCount];

		for (int lane = 0; lane < laneCount; lane++)
		{
			laneScores[lane] = -Math.Abs(lane - racer.Lane) * 0.15f;

			foreach (var item in items)
			{
				if (item.Collected || item.Lane != lane)
				{
					continue;
				}

				var distance = item.Position - racer.Progress;
				if (distance is <= 0f or > 20f)
				{
					continue;
				}

				if (item.Type == ItemType.Coin)
				{
					laneScores[lane] += 0.75f / distance;
				}
				else
				{
					laneScores[lane] -= 1.20f / distance;
				}
			}

			foreach (var other in racers)
			{
				if (other == racer || other.FinishPlace.HasValue || other.Lane != lane)
				{
					continue;
				}

				if (Math.Abs(other.Progress - racer.Progress) < 2.2f)
				{
					laneScores[lane] -= 0.6f;
				}
			}
		}

		var bestLane = 0;
		var bestScore = laneScores[0];
		for (int lane = 1; lane < laneCount; lane++)
		{
			if (laneScores[lane] > bestScore)
			{
				bestScore = laneScores[lane];
				bestLane = lane;
			}
		}

		// Small random chance to keep movement less predictable.
		if (random.NextDouble() < 0.07)
		{
			bestLane = Math.Clamp(bestLane + random.Next(-1, 2), 0, laneCount - 1);
		}

		if (bestLane > racer.Lane)
		{
			racer.Lane++;
		}
		else if (bestLane < racer.Lane)
		{
			racer.Lane--;
		}
	}
}

static void SpawnTrackItems(List<TrackItem> items, List<Racer> racers, Random random)
{
	items.RemoveAll(i => i.Collected || i.Position < racers.Min(r => r.Progress) - 5f);

	var lead = racers.Max(r => r.Progress);
	var farthestSpawn = items.Count == 0 ? lead : items.Max(i => i.Position);

	while (items.Count < 24 && farthestSpawn < trackLength - 3f)
	{
		var gap = random.Next(5, 11);
		var position = MathF.Min(trackLength - 2f, farthestSpawn + gap);
		var lane = random.Next(0, laneCount);
		var type = random.NextDouble() < 0.65 ? ItemType.Coin : ItemType.Obstacle;

		if (!items.Any(i => !i.Collected && i.Lane == lane && Math.Abs(i.Position - position) < 2f))
		{
			items.Add(new TrackItem(lane, position, type));
			farthestSpawn = position;
		}
		else
		{
			farthestSpawn += 1f;
		}
	}
}

static void MoveRacers(List<Racer> racers)
{
	foreach (var racer in racers)
	{
		if (!racer.FinishPlace.HasValue)
		{
			racer.PreviousProgress = racer.Progress;
			racer.Progress += racer.Speed;
		}
	}
}

static void ResolveItemCollisions(List<Racer> racers, List<TrackItem> items)
{
	foreach (var racer in racers)
	{
		if (racer.FinishPlace.HasValue)
		{
			continue;
		}

		foreach (var item in items)
		{
			if (item.Collected || item.Lane != racer.Lane)
			{
				continue;
			}

			var crossed = racer.PreviousProgress <= item.Position && racer.Progress >= item.Position;
			if (!crossed)
			{
				continue;
			}

			if (item.Type == ItemType.Coin)
			{
				racer.Speed = MathF.Min(maxSpeed, racer.Speed + 0.09f);
			}
			else
			{
				racer.Speed = MathF.Max(minSpeed, racer.Speed - 0.14f);
			}

			item.Collected = true;
		}
	}
}

static void ResolveRacerCollisions(List<Racer> racers)
{
	for (int i = 0; i < racers.Count; i++)
	{
		for (int j = i + 1; j < racers.Count; j++)
		{
			var a = racers[i];
			var b = racers[j];

			if (a.FinishPlace.HasValue || b.FinishPlace.HasValue)
			{
				continue;
			}

			var sameLane = a.Lane == b.Lane;
			var overlap = Math.Abs(a.Progress - b.Progress) < 0.55f;
			var ready = a.CollisionCooldown == 0 && b.CollisionCooldown == 0;

			if (sameLane && overlap && ready)
			{
				a.Speed = MathF.Max(minSpeed, a.Speed - 0.12f);
				b.Speed = MathF.Max(minSpeed, b.Speed - 0.12f);
				a.CollisionCooldown = 5;
				b.CollisionCooldown = 5;
			}
		}
	}
}

static void ResolveFinishers(List<Racer> racers, ref int nextFinishPlace, int tick, ref string? firstFinisher)
{
	foreach (var racer in racers)
	{
		if (!racer.FinishPlace.HasValue && racer.Progress >= trackLength)
		{
			racer.Progress = trackLength;
			racer.FinishPlace = nextFinishPlace++;
			racer.FinishTick = tick;
			if (firstFinisher is null)
			{
				firstFinisher = racer.Name;
			}
		}
	}
}

static void ApplySpeedNormalization(List<Racer> racers)
{
	foreach (var racer in racers)
	{
		if (racer.FinishPlace.HasValue)
		{
			continue;
		}

		racer.Speed += (baseSpeed - racer.Speed) * 0.03f;
		racer.Speed = Math.Clamp(racer.Speed, minSpeed, maxSpeed);
	}
}

static void DecrementCooldowns(List<Racer> racers)
{
	foreach (var racer in racers)
	{
		if (racer.CollisionCooldown > 0)
		{
			racer.CollisionCooldown--;
		}
	}
}

static string RenderFrame(List<Racer> racers, List<TrackItem> items, int tick, string? firstFinisher)
{
	var builder = new StringBuilder();
	builder.AppendLine($"Tick: {tick}   Track: {trackLength:0}   Controls: Left/Right");
	if (firstFinisher is not null)
	{
		builder.AppendLine($"First to finish: {firstFinisher}");
	}
	else
	{
		builder.AppendLine("First to finish: in progress");
	}

	builder.AppendLine();

	foreach (var racer in racers)
	{
		var finishText = racer.FinishPlace.HasValue ? $" FINISHED #{racer.FinishPlace}" : string.Empty;
		builder.AppendLine(
			$"{racer.Name,-7} lane:{racer.Lane + 1} speed:{racer.Speed:0.00} pos:{racer.Progress,6:0.0}/{trackLength:0}{finishText}");
	}

	builder.AppendLine();

	var laneRows = new char[laneCount][];
	for (int lane = 0; lane < laneCount; lane++)
	{
		laneRows[lane] = Enumerable.Repeat('.', renderWidth).ToArray();
		laneRows[lane][renderWidth - 1] = '|';
	}

	foreach (var item in items)
	{
		if (item.Collected)
		{
			continue;
		}

		var itemCol = ToColumn(item.Position);
		if (itemCol is >= 0 and < renderWidth - 1)
		{
			laneRows[item.Lane][itemCol] = item.Type == ItemType.Coin ? '$' : 'X';
		}
	}

	foreach (var racer in racers.OrderBy(r => r.Name == "Player" ? 0 : 1))
	{
		var racerCol = ToColumn(racer.Progress);
		if (racerCol >= renderWidth)
		{
			racerCol = renderWidth - 1;
		}

		laneRows[racer.Lane][racerCol] = racer.Symbol;
	}

	for (int lane = 0; lane < laneCount; lane++)
	{
		builder.AppendLine($"L{lane + 1} {new string(laneRows[lane])}");
	}

	builder.AppendLine();
	builder.AppendLine("Legend: P=Player 1/2=CPU  $=Coin  X=Obstacle  |=Finish");

	return builder.ToString();
}

static int ToColumn(float position)
{
	var clamped = Math.Clamp(position / trackLength, 0f, 1f);
	return (int)MathF.Round(clamped * (renderWidth - 1));
}

internal sealed class Racer(string name, char symbol, int lane, float speed)
{
	public string Name { get; } = name;
	public char Symbol { get; } = symbol;
	public int Lane { get; set; } = lane;
	public float Progress { get; set; }
	public float PreviousProgress { get; set; }
	public float Speed { get; set; } = speed;
	public int CollisionCooldown { get; set; }
	public int? FinishPlace { get; set; }
	public int? FinishTick { get; set; }
}

internal sealed class TrackItem(int lane, float position, ItemType type)
{
	public int Lane { get; } = lane;
	public float Position { get; } = position;
	public ItemType Type { get; } = type;
	public bool Collected { get; set; }
}

internal enum ItemType
{
	Coin,
	Obstacle,
}
