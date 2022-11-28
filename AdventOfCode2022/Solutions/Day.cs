namespace AdventOfCode2022.Solutions;

public abstract class Day
{
    protected abstract string InputFileName { get; }

    public string Solve()
    {
        var input = File.ReadAllLines($"Solutions/Input/{InputFileName}.txt");
        return Solve(input);
    }

    protected abstract string Solve(string[] input);
}