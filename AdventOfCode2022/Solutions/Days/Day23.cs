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
            if (map[y][x] == '#')locations.Add(new Elf { X = x, Y = y });
        }

        return locations.ToArray();
    }

    protected override object Solve(Elf[] input)
    {
        var firstDirection = 0;

        for (var repeat = 1;; repeat++)
        {
            var blockedVertical = new HashSet<(int, int)>();
            var blockedHorizontal = new HashSet<(int, int)>();
            var adjacent = new HashSet<(int, int)>();

            foreach (var elf in input)
            {
                blockedHorizontal.Add((elf.X, elf.Y - 1));
                blockedHorizontal.Add((elf.X, elf.Y));
                blockedHorizontal.Add((elf.X, elf.Y + 1));

                blockedVertical.Add((elf.X - 1, elf.Y));
                blockedVertical.Add((elf.X, elf.Y));
                blockedVertical.Add((elf.X + 1, elf.Y));

                adjacent.Add((elf.X - 1, elf.Y - 1));
                adjacent.Add((elf.X - 1, elf.Y));
                adjacent.Add((elf.X - 1, elf.Y + 1));

                adjacent.Add((elf.X, elf.Y - 1));
                adjacent.Add((elf.X, elf.Y + 1));

                adjacent.Add((elf.X + 1, elf.Y - 1));
                adjacent.Add((elf.X + 1, elf.Y));
                adjacent.Add((elf.X + 1, elf.Y + 1));
            }

            var newLocations = new Dictionary<(int x, int y), List<Elf>>();

            foreach (var elf in input)
            {
                if (!adjacent.Contains((elf.X, elf.Y)))
                {
                    continue;
                }

                for (var i = 0; i < 4; i++)
                {
                    var direction = (firstDirection + i) % 4;
                    var nextLocation = direction switch
                    {
                        0 => (elf.X, elf.Y - 1),
                        1 => (elf.X, elf.Y + 1),
                        2 => (elf.X - 1, elf.Y),
                        3 => (elf.X + 1, elf.Y),
                    };

                    var isVertical = direction == 0 || direction == 1;
                    if (isVertical && blockedVertical.Contains(nextLocation)) continue;
                    if (!isVertical && blockedHorizontal.Contains(nextLocation)) continue;

                    if (!newLocations.ContainsKey(nextLocation))
                    {
                        newLocations[nextLocation] = new List<Elf>();
                    }

                    newLocations[nextLocation].Add(elf);
                    break;
                }
            }

            var moves = newLocations.Where(x => x.Value.Count() == 1);
            var moveCount = moves.Count();

            foreach (var kvp in moves)
            {
                var elf = kvp.Value.Single();
                elf.X = kvp.Key.x;
                elf.Y = kvp.Key.y;
            }

            if (repeat == 10)
            {
                var rectSize = (input.Max(elf => elf.X) - input.Min(elf => elf.X) + 1)
                    * (input.Max(elf => elf.Y) - input.Min(elf => elf.Y) + 1);

                return rectSize - input.Count();
            };

            firstDirection = (firstDirection + 1) % 4;
        }
    }
}
