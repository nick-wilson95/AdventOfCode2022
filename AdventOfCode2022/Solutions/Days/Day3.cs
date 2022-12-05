using AdventOfCode2022.Utils;

namespace AdventOfCode2022.Solutions.Days;

public class Day3 : Day<IEnumerable<HashSet<char>>>
{
    protected override string InputFileName => "day3";
    
    protected override IEnumerable<HashSet<char>> Parse(string[] input) =>
        input.Select(x => x.ToHashSet());

    protected override object Solve(IEnumerable<HashSet<char>> input)
    {
        int Map(char x) => x < 97
            ? x % 32 + 26
            : x % 32;

        return input.Chunk(3)
            .Select(x => Set.Intersection(x).Single())
            .Select(Map)
            .Sum();
    }
}