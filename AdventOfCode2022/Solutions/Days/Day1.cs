using AdventOfCode2022.Solutions.Extensions;

namespace AdventOfCode2022.Solutions.Days;

public class Day1 : Day<IEnumerable<IEnumerable<int>>>
{
    protected override string InputFileName => "day1";

    protected override IEnumerable<IEnumerable<int>> Parse(string[] input) =>
        input.Split(string.Empty)
            .Select(x => x.Select(int.Parse));

    protected override string Solve(IEnumerable<IEnumerable<int>> input)
    {
        return input.Select(x => x.Sum())
            .Order()
            .TakeLast(3)
            .Sum()
            .ToString();
    }
}