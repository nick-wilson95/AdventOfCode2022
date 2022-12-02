using static AdventOfCode2022.Solutions.Days.Day2;
using static AdventOfCode2022.Solutions.Days.Day2.Option;

namespace AdventOfCode2022.Solutions.Days;

public class Day2 : Day<IEnumerable<(Option,Option)>>
{
    protected override string InputFileName => "day2";
    
    public enum Option { Rock, Paper, Scissors }

    protected override IEnumerable<(Option, Option)> Parse(string[] input) =>
        input.Select(x => (
            x[0] switch { 'A' => Rock, 'B' => Paper, 'C' => Scissors },
            x[2] switch { 'X' => Rock, 'Y' => Paper, 'Z' => Scissors }
        ));

    protected override string Solve(IEnumerable<(Option,Option)> input)
    {
        int ShapePoints((Option, Option) x) => (int)x.Item2 + 1;

        int OutcomePoints((Option, Option) x) => x switch
        {
            (Rock, Paper) or (Paper, Scissors) or (Scissors, Rock) => 6,
            (Rock, Rock) or (Paper, Paper) or (Scissors, Scissors) => 3,
            _ => 0,
        };

        return input.Select(x => ShapePoints(x) + OutcomePoints(x))
            .Sum()
            .ToString();
    }
}