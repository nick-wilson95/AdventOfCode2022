namespace AdventOfCode2022.Solutions.Days;

public class TestDay : Day<int[]>
{
    protected override string InputFileName => "test";
    
    protected override int[] Parse(string[] input)
    {
        return input.Select(int.Parse).ToArray();
    }

    protected override string Solve(int[] input)
    {
        return input.Sum()
            .ToString();
    }
}