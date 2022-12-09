using Array = AdventOfCode2022.Utils.Array;

namespace AdventOfCode2022.Solutions.Days;

public class Day9 : Day<IEnumerable<(char direction, int steps)>>
{
    protected override string InputFileName => "day9";

    protected override IEnumerable<(char direction, int steps)> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(' '))
            .Select(x => (x[0].Single(), int.Parse(x[1])));

    protected override object Solve(IEnumerable<(char direction, int steps)> input)
    {
        (int X, int Y)[] knots = Array.Of(_ => (0,0), 10);

        var tailVisited = new HashSet<(int,int)> { knots.Last() };

        int StepToward(int from, int target) => (target - from) switch
        {
            > 0 => from + 1,
            < 0 => from - 1,
            _ => from
        };

        foreach (var move in input)
            for (var i = 0; i < move.steps; i++)
            {
                knots[0].X += move.direction switch { 'L' => -1, 'R' => 1, _ => 0 };
                knots[0].Y += move.direction switch { 'D' => -1, 'U' => 1, _ => 0 };

                for (var k = 1; k < knots.Length; k++)
                {
                    if (Math.Abs(knots[k-1].X - knots[k].X) <= 1 && Math.Abs(knots[k-1].Y - knots[k].Y) <= 1) continue;
            
                    knots[k].X = StepToward(knots[k].X, knots[k-1].X);
                    knots[k].Y = StepToward(knots[k].Y, knots[k-1].Y);
                }
            
                tailVisited.Add(knots.Last()); 
            }

        return tailVisited.Count;
    }
}