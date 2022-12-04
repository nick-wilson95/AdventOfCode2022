using AdventOfCode2022.Extensions;

namespace AdventOfCode2022.Solutions.Days;

public class Day1 : Day<IEnumerable<IEnumerable<int>>>
{
    protected override string InputFileName => "day1";

    protected override IEnumerable<IEnumerable<int>> Parse(string[] input) =>
        input.Split(x => x == string.Empty)
            .Select(x => x.Select(int.Parse));

    protected override int Solve(IEnumerable<IEnumerable<int>> input) =>
        input.Select(x => x.Sum())
            .Order()
            .TakeLast(3)
            .Sum();
}