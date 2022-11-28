namespace AdventOfCode2022.Solutions.Days;

public class TestDay : Day
{
    protected override string InputFileName => "test";
    
    protected override string Solve(string[] input)
    {
        return input.SelectMany(x => x.Split(", "))
            .Select(int.Parse)
            .Sum()
            .ToString();
    }
}