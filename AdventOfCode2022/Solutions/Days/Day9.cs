namespace AdventOfCode2022.Solutions.Days;

public class Day9 : Day<IEnumerable<(char direction, int steps)>>
{
    protected override string InputFileName => "day9";

    protected override IEnumerable<(char direction, int steps)> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(' '))
            .Select(x => (x[0].Single(), int.Parse(x[1])));

    protected override object Solve(IEnumerable<(char direction, int steps)> input)
    {
        (int X, int Y) head = (0,0);
        (int X, int Y) tail = (0,0);

        var tailVisited = new HashSet<(int,int)> { tail };

        int StepToward(int from, int target) => (target - from) switch
        {
            > 0 => from + 1,
            < 0 => from - 1,
            _ => from
        };

        foreach (var move in input)
        {
            for (var i = 0; i < move.steps; i++)
            {
                head.X += move.direction switch { 'L' => -1, 'R' => 1, _ => 0 };
                head.Y += move.direction switch { 'D' => -1, 'U' => 1, _ => 0 };

                if (Math.Abs(head.X - tail.X) <= 1 && Math.Abs(head.Y - tail.Y) <= 1) continue;
                
                tail.X = StepToward(tail.X, head.X);
                tail.Y = StepToward(tail.Y, head.Y);
                
                tailVisited.Add(tail); 
            }
        }

        return tailVisited.Count;
    }
}