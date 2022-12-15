namespace AdventOfCode2022.Solutions.Days;

public class Day14 : Day<char[,]>
{
    protected override string InputFileName => "day14";
    
    protected override char[,] Parse(IEnumerable<string> input)
    {
        (int i,int j) ParseCoordinate(string input)
        {
            var parts = input.Split(',').Select(int.Parse).ToList();
            return (parts[0], parts[1]);
        }

        var walls = input.Select(line => line.Split(" -> ").Select(ParseCoordinate));

        var maxY = walls.SelectMany(x => x).Max(x => x.j);
        
        var wallSections = walls.SelectMany(x => x.SkipLast(1).Zip(x.Skip(1)));

        var wallGrid = new char[1000, maxY + 3];

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
                    wallGrid[from.i, j] = '#';
                }
            }
            else
            {
                var startX = Math.Min(from.i, to.i);
                var endX = Math.Max(from.i, to.i);

                for (var i = startX; i <= endX; i++)
                {
                    wallGrid[i, to.j] = '#';
                }
            }
        }

        for (var i = 0; i < 1000; i++) wallGrid[i, maxY + 2] = '#';

        return wallGrid;
    }

    protected override object Solve(char[,] input)
    {
        var particlesAtRest = 0;
        
        while (true)
        {
            (int i, int j) pos = (500, 0);

            while (true)
            {
                if (input[pos.i, pos.j + 1] == default) pos.j++;
                else if (input[pos.i - 1, pos.j + 1] == default) pos = (pos.i - 1, pos.j + 1);
                else if (input[pos.i + 1, pos.j + 1] == default) pos = (pos.i + 1, pos.j + 1);
                else {
                    input[pos.i, pos.j] = 'O';
                    particlesAtRest++;

                    if (pos == (500, 0))
                    {
                        //Render(input); //Uncomment the start of this line to render the final output
                        return particlesAtRest;
                    }
                    
                    break;
                }
            }
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