using Array = AdventOfCode2022.Utils.Array;

namespace AdventOfCode2022.Solutions.Days;

public class Day8 : Day<int[][]>
{
    protected override string InputFileName => "day8";
    
    protected override int[][] Parse(IEnumerable<string> input)
    {
        return input.Select(x => x.Select(ParseInt.FromChar).ToArray())
            .ToArray();
    }

    protected override object Solve(int[][] input)
    {
        var (gridHeight, gridWidth) = (input.Length, input.First().Length);
        
        var visibilityMatrix = Array.Of(_ => new bool[gridWidth], gridHeight);

        for (var i = 0; i < gridHeight; i++)
        {
            var currentMaxFromLeft = -1;
            var currentMaxFromRight = -1;
            
            for (var j = 0; j < gridWidth; j++)
            {
                if (input[i][j] > currentMaxFromLeft)
                {
                    currentMaxFromLeft = input[i][j];
                    visibilityMatrix[i][j] = true;
                }
                if (input[i][gridWidth - j - 1] > currentMaxFromRight)
                {
                    currentMaxFromRight = input[i][gridWidth - j - 1];
                    visibilityMatrix[i][gridWidth - j - 1] = true;
                }
            }
        }

        for (var j = 0; j < gridWidth; j++)
        {
            var currentMaxFromTop = -1;
            var currentMaxFromBottom = -1;
            
            for (var i = 0; i < gridHeight; i++)
            {
                if (input[i][j] > currentMaxFromTop)
                {
                    currentMaxFromTop = input[i][j];
                    visibilityMatrix[i][j] = true;
                }
                if (input[gridHeight - i - 1][j] > currentMaxFromBottom)
                {
                    currentMaxFromBottom = input[gridHeight - i - 1][j];
                    visibilityMatrix[gridHeight - i - 1][j] = true;
                }
            }
        }

        return visibilityMatrix.SelectMany(x => x).Count(x => x);
    }
}