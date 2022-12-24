using static AdventOfCode2022.Solutions.Days.Day24;

namespace AdventOfCode2022.Solutions.Days;

public class Day24 : Day<Blizzard[]>
{
    protected override string InputFileName => "day24";

    public class Blizzard
    {
        public char Direction { get; init; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    protected override Blizzard[] Parse(IEnumerable<string> input)
    {
        var inputArray = input.ToArray();

        var blizzards = new List<Blizzard>();

        for (var y = 0; y < inputArray.Length; y ++)
        for (var x = 0; x < inputArray.First().Length; x++)
        {
            if (inputArray[y][x] is '>' or '<' or 'v' or '^')
            {
                    blizzards.Add(new Blizzard { Direction = inputArray[y][x], X = x - 1, Y = y - 1 });
            }
        }

        return blizzards.ToArray();
    }

    protected override object Solve(Blizzard[] input)
    {
        var width = input.Select(b => b.X).Max() + 1;
        var length = input.Select(b => b.Y).Max() + 1;

        var numBlizzards = new int[width, length];
        foreach (var blizzard in input) numBlizzards[blizzard.X, blizzard.Y]++;

        var weatherCycleLength = width * length / GCD(width, length);

        var visited = new List<(int cycleTime, int x, int y)>();

        var start = (0, -1);
        var target = (width - 1, length - 1);

        var lastPositions = new (int x, int y)[] { start };

        var time = 0;

        while(true)
        {
            time++;

            if (lastPositions.Any(x => x == target)) return time;

            foreach (var blizzard in input)
            {
                numBlizzards[blizzard.X, blizzard.Y]--;

                blizzard.X = blizzard.Direction switch
                {
                    '>' => (blizzard.X + 1) % width,
                    '<' => (blizzard.X + width - 1) % width,
                    _ => blizzard.X
                };

                blizzard.Y = blizzard.Direction switch
                {
                    'v' => (blizzard.Y + 1) % length,
                    '^' => (blizzard.Y + length - 1) % length,
                    _ => blizzard.Y
                };

                numBlizzards[blizzard.X, blizzard.Y]++;
            }

            var nextPositions = lastPositions.SelectMany(pos =>
                {
                    var adjacent = new (int x, int y)[] {
                        (pos.x - 1, pos.y),
                        (pos.x, pos.y - 1),
                        (pos.x, pos.y),
                        (pos.x + 1, pos.y),
                        (pos.x, pos.y + 1)
                    };

                    return adjacent.Where(p => (p.x >= 0 && p.y >= 0 && p.x < width && p.y < length) || p == start)
                        .Where(p => p == start || numBlizzards[p.x, p.y] == 0)
                        .Where(p => !visited.Contains((time % weatherCycleLength, p.x, p.y)));
                })
                .Distinct()
                .ToArray();

            foreach (var pos in nextPositions)
            {
                visited.Add((time % weatherCycleLength, pos.x, pos.y));
            }

            lastPositions = nextPositions.ToArray();
        }
    }

    private static int GCD(int a, int b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }
}
