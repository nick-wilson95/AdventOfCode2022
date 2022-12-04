using AdventOfCode2022.Extensions;

namespace AdventOfCode2022.Solutions.Days;

public class Day4 : Day<IEnumerable<(Range,Range)>>
{
    protected override string InputFileName => "day4";
    
    protected override IEnumerable<(Range,Range)> Parse(string[] input)
    {
        return input.Select(x => x.Split(',','-'))
            .Select(x => x.Select(int.Parse).ToList())
            .Select(x => (new Range(x[0],x[1]), new Range(x[2],x[3])));
    }

    protected override string Solve(IEnumerable<(Range,Range)> input)
    {
        return input.Count(x =>
                x.Item1.Contains(x.Item2) || x.Item2.Contains(x.Item1))
            .ToString();
    }
}