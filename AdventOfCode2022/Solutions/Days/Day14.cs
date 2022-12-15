namespace AdventOfCode2022.Solutions.Days;

public class Day14 : Day<Day14.CaveData>
{
    protected override string InputFileName => "day14";
    
    public record CaveData(char[,] Walls, (int i, int j) Source){}
    
    protected override CaveData Parse(IEnumerable<string> input)
    {
        (int i,int j) ParseCoordinate(string input)
        {
            var parts = input.Split(',').Select(int.Parse).ToList();
            return (parts[0], parts[1]);
        }

        var walls = input.Select(line => line.Split(" -> ").Select(ParseCoordinate));

        var wallEnds = walls.SelectMany(x => x);
        var minX = wallEnds.Min(x => x.i);
        var maxX = wallEnds.Max(x => x.i);
        var maxY = wallEnds.Max(x => x.j);

        var xOffset = minX - 1;
        
        var wallSections = walls.SelectMany(x => x.SkipLast(1).Zip(x.Skip(1)));

        var wallGrid = new char[maxX - minX + 2, maxY + 2];

        foreach (var section in wallSections)
        {
            var (from, to) = section;
            
            var isHorizontal = from.i == to.i;

            if (isHorizontal)
            {
                var startY = Math.Min(from.j, to.j);
                var endY = Math.Max(from.j, to.j);

                for (var j = startY; j <= endY; j++)
                {
                    wallGrid[from.i - xOffset, j] = '#';
                }
            }
            else
            {
                var startX = Math.Min(from.i, to.i);
                var endX = Math.Max(from.i, to.i);

                for (var i = startX; i <= endX; i++)
                {
                    wallGrid[i - xOffset, to.j] = '#';
                }
            }
        }

        return new CaveData(wallGrid, (500 - xOffset, 0));
    }

    protected override object Solve(CaveData input)
    {
        var particlesAtRest = 0;
        var length = input.Walls.GetLength(1);
        
        while (true)
        {
            var pos = input.Source;

            while (true)
            {
                if (pos.j >= length - 1)
                {
                    Render(input.Walls);
                    return particlesAtRest;
                }
                
                if (input.Walls[pos.i, pos.j + 1] == default) pos.j++;
                else if (input.Walls[pos.i - 1, pos.j + 1] == default) pos = (pos.i - 1, pos.j + 1);
                else if (input.Walls[pos.i + 1, pos.j + 1] == default) pos = (pos.i + 1, pos.j + 1);
                else {
                    input.Walls[pos.i, pos.j] = 'O';
                    break;
                }
            }
            
            particlesAtRest++;
        }
    }

    private static void Render(char[,] input)
    {
        Console.Clear();
        
        var width = input.GetLength(0);
        var length = input.GetLength(1);

        for (var j = 0; j < length; j++)
        {
            var row = "";
            for (var i = 0; i < width; i++) row += input[i, j];
            Console.WriteLine(row);
        }
    }
}