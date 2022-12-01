using AdventOfCode2022.Solutions.Extensions;

namespace AdventOfCode2022.Solutions.Days;

public class Day1 : Day<IEnumerable<int?>>
{
    protected override string InputFileName => "day1";

    protected override IEnumerable<int?> Parse(string[] input) =>
        input.Select(x => int.TryParse(x, out var value) ? (int?)value : null);

    protected override string Solve(IEnumerable<int?> input)
    {
        return input.Split(null)
            .Select(x => (int)x.Sum())
            .Order()
            .TakeLast(3)
            .Sum()
            .ToString();
    }
}