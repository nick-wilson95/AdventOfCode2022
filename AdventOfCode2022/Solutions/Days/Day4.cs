namespace AdventOfCode2022.Solutions.Days;

public record IntRange(int From, int To)
{
    public bool Overlaps(IntRange other) => !(To < other.From || From > other.To);
}

public class Day4 : Day<IEnumerable<(IntRange,IntRange)>>
{
    protected override string InputFileName => "day4";
    
    protected override IEnumerable<(IntRange,IntRange)> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(',','-'))
            .Select(x => x.Select(int.Parse).ToList())
            .Select(x => (new IntRange(x[0],x[1]), new IntRange(x[2],x[3])));

    protected override object Solve(IEnumerable<(IntRange, IntRange)> input) =>
        input.Count(x => x.Item1.Overlaps(x.Item2));
}