namespace AdventOfCode2022.Solutions;

public abstract class Day<TInput>
{
    protected abstract string InputFileName { get; }

    public string Solve()
    {
        var input = File.ReadAllLines($"Solutions/Inputs/{InputFileName}.txt");
        return Solve(Parse(input));
    }

    protected abstract TInput Parse(string[] input);

    protected abstract string Solve(TInput input);
}