namespace AdventOfCode2022.Solutions.Days;

public class Day19 : Day<IEnumerable<int[,]>>
{
    protected override string InputFileName => "day19";

    protected override IEnumerable<int[,]> Parse(IEnumerable<string> input) =>
        input.Select(line =>
        {
            var parts = line.Split(' ');
            
            var numericParts = new[] { 6, 12, 18, 21, 27, 30 }
                .Select(x => parts[x])
                .Select(int.Parse)
                .ToArray();

            return new[,]
            {
                { numericParts[0], 0,               0               },
                { numericParts[1], 0,               0               },
                { numericParts[2], numericParts[3], 0               },
                { numericParts[4], 0,               numericParts[5] }
            };
        });


    protected override object Solve(IEnumerable<int[,]> input)
    {
        var sum = 0;

        Parallel.ForEach(input.Zip(Enumerable.Range(1, input.Count())), x =>
        {
            var geodes = BestGeodeCount(x.First);
            sum += geodes * x.Second;

            Console.WriteLine($"Blueprint {x.Second} - {geodes} geodes");
        });

        return sum;
    }

    private static int BestGeodeCount(int[,] costs)
    {
        var bestGeodeCount = 0;

        var maxOreCost = new[] { costs[0, 0], costs[1, 0], costs[2, 0], costs[3, 0] }.Max();

        void Recurse(int elapsed, int[] robots, int[] resources)
        {
            var nextResources = resources.Zip(robots, (x, y) => x + y).ToArray();

            if (elapsed == 23)
            {
                if (nextResources[3] > bestGeodeCount) bestGeodeCount = nextResources[3];
                return;
            }

            var canBuildOre = resources[0] >= costs[0, 0] && resources[0] < (24 - elapsed) * maxOreCost;
            var canBuildClay = resources[0] >= costs[1, 0] && resources[1] < (24 - elapsed) * costs[2, 1];
            var canBuildObsidian = resources[0] >= costs[2, 0] && resources[1] >= costs[2, 1] && resources[2] < (24 - elapsed) * costs[3, 2];
            var canBuildGeode = resources[0] >= costs[3, 0] && resources[2] >= costs[3, 2];

            if (canBuildOre)
            {
                Recurse(
                    elapsed + 1,
                    new[] { robots[0] + 1, robots[1], robots[2], robots[3] },
                    new[] { nextResources[0] - costs[0, 0], nextResources[1], nextResources[2], nextResources[3] }
                );
            }
            if (canBuildClay)
            {
                Recurse(
                    elapsed + 1,
                    new[] { robots[0], robots[1] + 1, robots[2], robots[3] },
                    new[] { nextResources[0] - costs[1, 0], nextResources[1], nextResources[2], nextResources[3] }
                );
            }
            if (canBuildObsidian)
            {
                Recurse(
                    elapsed + 1,
                    new[] { robots[0], robots[1], robots[2] + 1, robots[3] },
                    new[] { nextResources[0] - costs[2, 0], nextResources[1] - costs[2, 1], nextResources[2], nextResources[3] }
                );
            }
            if (canBuildGeode)
            {
                Recurse(
                    elapsed + 1,
                    new[] { robots[0], robots[1], robots[2], robots[3] + 1 },
                    new[] { nextResources[0] - costs[3, 0], nextResources[1], nextResources[2] - costs[3, 2], nextResources[3] }
                );
            }

            if (resources[0] <= maxOreCost)
            {
                Recurse(elapsed + 1, robots, nextResources);
            }
        }

        Recurse(0, new[] { 1, 0, 0, 0 }, new[] { 0, 0, 0, 0 });

        return bestGeodeCount;
    }
}