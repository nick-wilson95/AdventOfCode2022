namespace AdventOfCode2022.Solutions.Days;

public class Day12 : Day<Day12.MapInput>
{
    protected override string InputFileName => "day12";
    
    public record MapInput((int i,int j) Start, (int i,int j) Target, int[][] Grid){}
    
    protected override MapInput Parse(IEnumerable<string> input)
    {
        var list = input.ToList();
        
        var start = (0, 0);
        var end = (0, 0);
        
        for (var i = 0; i < list.Count; i++)
        for (var j = 0; j < list.First().Length; j++)
        {
            if (list[i][j] == 'S')
            {
                start = (i, j);
                list[i] = list[i].Replace('S', 'a');
            }

            if (list[i][j] == 'E')
            {
                end = (i, j);
                list[i] = list[i].Replace('E', 'z');
            }
        }

        var grid = list.Select(line => line.Select(c => c % 32).ToArray())
            .ToArray();

        return new MapInput(start, end, grid);
    }

    protected override object Solve(MapInput input)
    {
        var stepsTaken = 0;

        var visited = new HashSet<(int i, int j)>();
        var lastStep = new HashSet<(int i, int j)> { input.Start };

        while (true)
        {
            if (lastStep.Contains(input.Target)) return stepsTaken;
            
            var thisStep = new  HashSet<(int i, int j)>();
            
            foreach (var from in lastStep)
            {
                bool CanStep((int i, int j) to)
                {
                    var inBounds = to.i >= 0 && to.i < input.Grid.Length
                                && to.j >= 0 && to.j < input.Grid.First().Length;

                    bool LowEnough() => input.Grid[to.i][to.j] <= input.Grid[from.i][from.j] + 1;

                    return inBounds && LowEnough() && !visited.Contains(to);
                }

                var nextSteps = new (int i, int j)[]
                {
                    (from.i, from.j + 1),
                    (from.i, from.j - 1),
                    (from.i + 1, from.j),
                    (from.i - 1, from.j),
                }.Where(CanStep);

                thisStep = thisStep.Concat(nextSteps).ToHashSet();
            }

            visited = visited.Concat(lastStep).ToHashSet();
            lastStep = thisStep;
            
            stepsTaken++;
        }
    }
}