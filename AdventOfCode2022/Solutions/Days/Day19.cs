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


    protected override object Solve(IEnumerable<int[,]> input) =>
        input.Select(BestGeodeCount)
            .Zip(Enumerable.Range(1, input.Count()))
            .Select(x => x.First * x.Second)
            .Sum();

    private int BestGeodeCount(int[,] costs)
    {
        var bestGeodeCount = 0;
        
        for (var targetOreRobots = 0;; targetOreRobots++)
        for (var targetClayRobots = 0;; targetClayRobots++)
        for (var targetObsidianRobots = 0;; targetObsidianRobots++)
        {
            var robots = new[]{ 1, 0, 0, 0 };
            var resources = new[]{ 0, 0, 0, 0 };
        
            for (var i = 0; i < 24; i++)
            {
                resources = resources.Zip(robots, (x, y) => x + y).ToArray();

                if (robots[0] < targetOreRobots && resources[0] > costs[0, 0])
                {
                    resources[0] -= costs[0, 0];
                    robots[0]++;
                    continue;
                }

                if (robots[1] < targetClayRobots && resources[0] > costs[1, 0])
                {
                    resources[0] -= costs[1, 0];
                    robots[1]++;
                    continue;
                }

                if (robots[2] < targetObsidianRobots && resources[0] > costs[2, 0] && resources[1] > costs[2, 1])
                {
                    resources[0] -= costs[2, 0];
                    resources[1] -= costs[2, 1];
                    robots[2]++;
                    continue;
                }

                if (resources[0] > costs[3, 0] && resources[2] > costs[3, 2])
                {
                    resources[0] -= costs[3, 0];
                    resources[2] -= costs[3, 2];
                    robots[3]++;
                }
            }

            if (resources[3] > bestGeodeCount) bestGeodeCount = resources[3];
        }

        return bestGeodeCount;
    }
}