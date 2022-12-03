namespace AdventOfCode2022.Solutions.Days;

public class Day3 : Day<IEnumerable<(HashSet<char>,HashSet<char>)>>
{
    protected override string InputFileName => "day3";
    
    protected override IEnumerable<(HashSet<char>,HashSet<char>)> Parse(string[] input)
    {
        return input.Select(x =>
        (
            x[..(x.Length/2)].ToHashSet(),
            x[(x.Length/2)..].ToHashSet()
        ));
    }

    protected override string Solve(IEnumerable<(HashSet<char>,HashSet<char>)> input)
    {
        int Map(char x) => x < 97
            ? x % 32 + 26
            : x % 32;

        return input.Select(x => x.Item1.Intersect(x.Item2).Single())
            .Select(Map)
            .Sum()
            .ToString();
    }
}