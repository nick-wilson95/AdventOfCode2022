namespace AdventOfCode2022.Solutions;

public abstract class Day
{
    public abstract string Solve();
}

public abstract class Day<TInput> : Day
{
    protected abstract string InputFileName { get; }

    public override string Solve()
    {
        var input = File.ReadAllLines($"Solutions/Inputs/{InputFileName}.txt");
        return Solve(Parse(input)).ToString();
    }

    // No holds barred string wrangling
    protected abstract TInput Parse(IEnumerable<string> input);

    // This bit should be clean
    protected abstract object Solve(TInput input);
}