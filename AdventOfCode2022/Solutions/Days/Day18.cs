namespace AdventOfCode2022.Solutions.Days;

public class Day18 : Day<IEnumerable<(int x, int y, int z)>>
{
    protected override string InputFileName => "day18";
    
    protected override IEnumerable<(int x, int y, int z)> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(',').Select(int.Parse).ToArray())
            .Select(x => (x[0],x[1],x[2]));

    protected override object Solve(IEnumerable<(int x, int y, int z)> input)
    {
        var neighbours = new Dictionary<(int x, int y, int z), int>();

        foreach (var p in input)
        {
            if (!neighbours.ContainsKey(p)) neighbours[p] = 0;
            
            var adjacent = new[]
            {
                (p.x + 1, p.y, p.z),
                (p.x - 1, p.y, p.z),
                (p.x, p.y + 1, p.z),
                (p.x, p.y - 1, p.z),
                (p.x, p.y, p.z + 1),
                (p.x, p.y, p.z - 1)
            };

            foreach (var q in adjacent)
            {
                if (!neighbours.ContainsKey(q)) neighbours[q] = 0;
                neighbours[q]++;
            }
        }

        return input.Select(x => 6 - neighbours[x]).Sum();
    }
}