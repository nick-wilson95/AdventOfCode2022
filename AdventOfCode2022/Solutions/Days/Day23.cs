namespace AdventOfCode2022.Solutions.Days;

public class Elf
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class Day23 : Day<Elf[]>
{
    protected override string InputFileName => "day23";

    protected override Elf[] Parse(IEnumerable<string> input)
    {
        var locations = new List<Elf>();

        var map = input.ToArray();

        for (var y = 0; y < map.Length; y++)
        for (var x = 0; x < map[0].Length; x ++)
        {
            if (map[y][x] == '#') locations.Add(new Elf { X = x, Y = y });
        }

        return locations.ToArray();
    }

    protected override object Solve(Elf[] input)
    {
        var firstDirection = 0;

        for (var repeat = 1;; repeat++)
        {
            var blockedHorizontal = input.SelectMany(elf => new[] {
                (elf.X, elf.Y - 1),
                (elf.X, elf.Y),
                (elf.X, elf.Y + 1)
            }).ToHashSet();

            var blockedVertical = input.SelectMany(elf => new[] {
                (elf.X - 1, elf.Y),
                (elf.X, elf.Y),
                (elf.X + 1, elf.Y)
            }).ToHashSet();

            var adjacent = input.SelectMany(elf => new[]
            {
                (elf.X - 1, elf.Y - 1),
                (elf.X - 1, elf.Y),
                (elf.X - 1, elf.Y + 1),
                (elf.X, elf.Y - 1),
                (elf.X, elf.Y + 1),
                (elf.X + 1, elf.Y - 1),
                (elf.X + 1, elf.Y),
                (elf.X + 1, elf.Y + 1)
            }).ToHashSet();

            var proposedLocations = new Dictionary<(int x, int y), List<Elf>>();

            foreach (var elf in input)
            {
                if (!adjacent.Contains((elf.X, elf.Y)))
                {
                    continue;
                }

                for (var i = 0; i < 4; i++)
                {
                    var direction = (firstDirection + i) % 4;
                    var location = direction switch
                    {
                        0 => (elf.X, elf.Y - 1),
                        1 => (elf.X, elf.Y + 1),
                        2 => (elf.X - 1, elf.Y),
                        3 => (elf.X + 1, elf.Y),
                    };

                    var isVertical = direction == 0 || direction == 1;
                    if (isVertical && blockedVertical.Contains(location)) continue;
                    if (!isVertical && blockedHorizontal.Contains(location)) continue;

                    if (!proposedLocations.ContainsKey(location))
                    {
                        proposedLocations[location] = new List<Elf>();
                    }

                    proposedLocations[location].Add(elf);
                    break;
                }
            }

            var moves = proposedLocations.Where(x => x.Value.Count == 1);

            if (!moves.Any()) return repeat;

            foreach (var kvp in moves)
            {
                var elf = kvp.Value.Single();
                elf.X = kvp.Key.x;
                elf.Y = kvp.Key.y;
            }

            firstDirection = (firstDirection + 1) % 4;
        }
    }
}
