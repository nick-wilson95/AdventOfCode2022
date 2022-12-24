using static AdventOfCode2022.Solutions.Days.Day24;

namespace AdventOfCode2022.Solutions.Days;

public class Day24 : Day<((int x, int y) start, (int x, int y) end, Blizzard[] blizzards)>
{
    protected override string InputFileName => "day24";

    public class Blizzard
    {
        public char Direction { get; init; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    protected override ((int x, int y) start, (int x, int y) end, Blizzard[] blizzards) Parse(IEnumerable<string> input)
    {
        var inputArray = input.ToArray();

        (int x, int y) start = default;
        (int x, int y) end = default;
        var blizzards = new List<Blizzard>();

        for (var y = 0; y < inputArray.Length; y++)
            for (var x = 0; x < inputArray.First().Length; x++)
            {
                var character = inputArray[y][x];

                if (y == 0 && character == '.') start = (x - 1, y - 1);
                if (y == inputArray.Length - 1 && character == '.') end = (x - 1, y - 1);

                if (character is '>' or '<' or 'v' or '^')
                {
                    blizzards.Add(new Blizzard { Direction = character, X = x - 1, Y = y - 1 });
                }
            }

        return (start, end, blizzards.ToArray());
    }

    protected override object Solve(((int x, int y) start, (int x, int y) end, Blizzard[] blizzards) input)
    {
        var (start, end, blizzards) = input;

        var width = blizzards.Select(b => b.X).Max() + 1;
        var length = blizzards.Select(b => b.Y).Max() + 1;

        bool InBounds((int x, int y) q) =>
            q.x >= 0 && q.y >= 0 && q.x < width && q.y < length;

        var intensity = new int[width, length];
        foreach (var blizzard in blizzards)
        {
            intensity[blizzard.X, blizzard.Y]++;
        }

        var weatherCycleLength = width * length / IntMath.GCD(width, length);

        var visited = new HashSet<(int cycleTime, int x, int y)>();

        var targets = new Queue<(int x, int y)>(new [] { end, start, end });

        var target = targets.Dequeue();

        var lastPositions = new (int x, int y)[] { start };

        for (var time = 0;; time++)
        {
            if (lastPositions.Any(x => x == target))
            {
                lastPositions = new (int x, int y)[] { target };

                if (!targets.TryDequeue(out target)) return time;

                visited.Clear();
            }

            foreach (var bliz in blizzards)
            {
                intensity[bliz.X, bliz.Y]--;

                (bliz.X, bliz.Y) = bliz.Direction switch
                {
                    '>' => ((bliz.X + 1) % width, bliz.Y),
                    '<' => ((bliz.X + width - 1) % width, bliz.Y),
                    'v' => (bliz.X, (bliz.Y + 1) % length),
                    '^' => (bliz.X, (bliz.Y + length - 1) % length),
                    _ => (bliz.X, bliz.Y)
                };

                intensity[bliz.X, bliz.Y]++;
            }

            var nextPositions = lastPositions.SelectMany(p =>
            {
                var adjacent = new (int x, int y)[]
                {
                    (p.x - 1, p.y),
                    (p.x, p.y - 1),
                    (p.x, p.y),
                    (p.x, p.y + 1),
                    (p.x + 1, p.y)
                };

                return adjacent.Where(q => (InBounds(q) && intensity[q.x, q.y] == 0) || q == start || q == end)
                    .Where(q => !visited.Contains((time % weatherCycleLength, q.x, q.y)));
            }).Distinct().ToArray();

            foreach (var (x, y) in nextPositions)
            {
                visited.Add((time % weatherCycleLength, x, y));
            }

            lastPositions = nextPositions.ToArray();
        }
    }
}
