namespace AdventOfCode2022.Solutions.Days;

public class Day10 : Day<IEnumerable<int>>
{
    protected override string InputFileName => "day10";
    
    protected override IEnumerable<int> Parse(IEnumerable<string> input)
    {
        int GetValue(string line) => int.Parse(line.Split(' ').Last());

        return input.SelectMany(x => x == "noop"
                ? new[] { 0 }
                : new[] { 0, GetValue(x) })
            .Prepend(1)
            .Cumulate();
    }

    protected override object Solve(IEnumerable<int> input)
    {
        return input.Select((y, i) => Math.Abs(y - i % 40) <= 1 ? '#' : '.')
            .Chunk(40)
            .Take(6)
            .Select(x => new string(x))
            .Aggregate((x,y) => x + "\r\n" + y);
    }
}