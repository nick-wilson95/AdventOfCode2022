using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2022.Solutions.Days;

public class Job
{
    public BigInteger? Value { get; private set; }

    public string First { get; }

    public string Second { get; }

    private readonly Func<Dictionary<string, Job>, BigInteger?> _getValue;

    private readonly string[] _parts;

    public Job(Func<Dictionary<string, Job>, BigInteger?> getValue, string[] parts)
    {
        _getValue = getValue;
        _parts = parts;

        if (parts.Count() > 3)
        {
            First = parts[2];
            Second = parts[4];
        }
    }

    public BigInteger? GetValue(Dictionary<string, Job> input)
    {
        var value = _getValue(input);
        Value = value;
        return value;
    }

    public void SetValue(Dictionary<string, Job> input, BigInteger value)
    {
        if (Value.HasValue) throw new UnreachableException();

        Value = value;

        if (_parts[0] == "humn") return;

        var (first, function, second) = (_parts[2], _parts[3], _parts[4]);

        var firstValue = input[first].Value;
        var secondValue = input[second].Value;

        if (firstValue.HasValue)
        {
            var newValue = function switch
            {
                "+" => value - firstValue,
                "-" => firstValue - value,
                "/" => firstValue / value,
                "*" => value / firstValue,
            };

            input[second].SetValue(input, newValue.Value);
        }
        else
        {
            var newValue = function switch
            {
                "+" => value - secondValue,
                "-" => value + secondValue,
                "/" => value * secondValue,
                "*" => value / secondValue,
            };

            input[first].SetValue(input, newValue.Value);
        }
    }

    public static Job Parse(string[] parts)
    {
        if (parts[0] == "humn")
            return new Job(_ => null, parts);

        if (parts.Count() == 3)
            return new Job(_ => BigInteger.Parse(parts[2]), parts);

        Func<BigInteger, BigInteger, BigInteger> function = parts[3] switch
        {
            "+" => (x,y) => x + y,
            "-" => (x, y) => x - y,
            "/" => (x, y) => x / y,
            "*" => (x, y) => x * y,
        };

        return new Job(x => {
            var value1 = x[parts[2]].GetValue(x);
            var value2 = x[parts[4]].GetValue(x);

            if (!value1.HasValue || !value2.HasValue) return null;

            return function(value1.Value, value2.Value);
        }, parts);
    }
}

public class Day21 : Day<Dictionary<string, Job>>
{
    protected override string InputFileName => "day21";

    protected override Dictionary<string, Job> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(':', ' '))
            .ToDictionary(x => x[0], x => Job.Parse(x.ToArray()));

    protected override object Solve(Dictionary<string, Job> input)
    {
        var root = input["root"];

        root.GetValue(input);

        var value1 = input[root.First].Value;
        var value2 = input[root.Second].Value;

        if (value1.HasValue)
        {
            input[root.Second].SetValue(input, value1.Value);
        }
        else
        {
            input[root.First].SetValue(input, value2.Value);
        }

        return input["humn"].Value.Value;
    }
}
