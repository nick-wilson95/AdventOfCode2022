using System.Numerics;

namespace AdventOfCode2022.Solutions.Days;

public class Job
{
    private BigInteger? _cache = null;
    private readonly Func<Dictionary<string, Job>, BigInteger> _getValue;

    public Job(Func<Dictionary<string, Job>, BigInteger> getValue)
    {
        _getValue = getValue;
    }

    public BigInteger GetValue(Dictionary<string, Job> input) =>
        _cache ?? _getValue(input);

    public static Job Parse(string[] parts)
    {
        if (parts.Count() == 1)
            return new Job(_ => BigInteger.Parse(parts[0]));

        Func<BigInteger, BigInteger, BigInteger> function = parts[1] switch
        {
            "+" => (x,y) => x + y,
            "-" => (x, y) => x - y,
            "/" => (x, y) => x / y,
            "*" => (x, y) => x * y,
        };

        return new Job(x => function(x[parts[0]].GetValue(x), x[parts[2]].GetValue(x)));
    }
}

public class Day21 : Day<Dictionary<string, Job>>
{
    protected override string InputFileName => "day21";

    protected override Dictionary<string, Job> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(':', ' '))
            .ToDictionary(x => x[0], x => Job.Parse(x.Skip(2).ToArray()));

    protected override object Solve(Dictionary<string, Job> input) =>
        input["root"].GetValue(input);
}
