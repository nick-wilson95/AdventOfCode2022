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
        var lastStep = new HashSet<(int i, int j)> { input.Target };
        
        bool CanStep((int i, int j) from, (int i, int j) to)
        {
            var highEnough = input.Grid[to.i][to.j] >= input.Grid[from.i][from.j] - 1;

            return highEnough && !visited.Contains(to);
        }

        while (true)
        {
            if (lastStep.Any(x => input.Grid[x.i][x.j] == 1)) return stepsTaken;

            var thisStep = new HashSet<(int i, int j)>();

            foreach (var from in lastStep)
            {
                var nextSteps = GetAdjacent(input.Grid, from).Where(to => CanStep(from, to));

                thisStep = thisStep.Concat(nextSteps).ToHashSet();
            }

            visited = visited.Concat(lastStep).ToHashSet();
            lastStep = thisStep;

            stepsTaken++;
        }
    }

    private static IEnumerable<(int i, int j)> GetAdjacent(int[][] grid, (int i, int j) from)
    {
        bool InBounds((int i, int j) pos) => pos.i >= 0 && pos.i < grid.Length
                                     && pos.j >= 0 && pos.j < grid.First().Length;
        
        return new (int i, int j)[]
        {
            (from.i, from.j + 1),
            (from.i, from.j - 1),
            (from.i + 1, from.j),
            (from.i - 1, from.j),
        }.Where(InBounds);
    }
}