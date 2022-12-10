using Array = AdventOfCode2022.Utils.Array;

namespace AdventOfCode2022.Solutions.Days;

public class Day10 : Day<List<int>>
{
    protected override string InputFileName => "day10";
    
    protected override List<int> Parse(IEnumerable<string> input)
    {
        var result = new List<int>{1};

        foreach (var line in input)
        {
            result.Add(result.Last());
            if (line == "noop") continue;
            
            var lineValue = int.Parse(line.Split(' ').Last());
            result.Add(result.Last() + lineValue);
        }

        return result;
    }

    protected override object Solve(List<int> input)
    {
        return Array.Of(i => 20 + 40 * i, 6)
            .Select(x => x * input[x - 1])
            .Sum();
    }
}