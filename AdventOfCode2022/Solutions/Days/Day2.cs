using AdventOfCode2022.Extensions;
using static AdventOfCode2022.Solutions.Days.Day2;
using static AdventOfCode2022.Solutions.Days.Day2.Option;
using static AdventOfCode2022.Solutions.Days.Day2.Outcome;

namespace AdventOfCode2022.Solutions.Days;

public class Day2 : Day<IEnumerable<(Option,Outcome)>>
{
    protected override string InputFileName => "day2";
    
    public enum Option { Rock, Paper, Scissors }
    public enum Outcome { Lose, Draw, Win }

    protected override IEnumerable<(Option, Outcome)> Parse(IEnumerable<string> input) =>
        input.Select(x => (
            x[0] switch { 'A' => Rock, 'B' => Paper, 'C' => Scissors },
            x[2] switch { 'X' => Lose, 'Y' => Draw, 'Z' => Win }
        ));

    protected override object Solve(IEnumerable<(Option,Outcome)> input)
    {
        int ShapePoints((Option, Outcome) x) => ((int)x.Item1 + (int)x.Item2 - 1).Mod(3) + 1;

        int OutcomePoints((Option, Outcome) x) => x.Item2 switch { Lose => 0, Draw => 3, Win => 6 };

        return input.Select(x => ShapePoints(x) + OutcomePoints(x))
            .Sum();
    }
}