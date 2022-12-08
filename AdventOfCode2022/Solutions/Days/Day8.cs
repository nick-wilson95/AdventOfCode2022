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
        
        var scenicScores = Array.Of(_ => Array.Of(_ => 1, gridWidth), gridHeight);

        for (var i = 1; i < gridHeight - 1; i++)
        {
            for (var j = 1; j < gridWidth - 1; j++)
            {
                for (var k = j - 1; k >= 0; k--)
                {
                    if (input[i][k] >= input[i][j] || k == 0)
                    {
                        scenicScores[i][j] *= j - k;
                        break;
                    }
                }
                for (var k = j + 1; k < gridWidth; k++)
                {
                    if (input[i][k] >= input[i][j] || k == gridWidth - 1)
                    {
                        scenicScores[i][j] *= k - j;
                        break;
                    }
                }
            }
        }

        for (var i = 1; i < gridWidth - 1; i++)
        {
            for (var j = 1; j < gridHeight - 1; j++)
            {
                for (var k = i - 1; k >= 0; k--)
                {
                    if (input[k][j] >= input[i][j] || k == 0)
                    {
                        scenicScores[i][j] *= i - k;
                        break;
                    }
                }
                for (var k = i + 1; k < gridHeight; k++)
                {
                    if (input[k][j] >= input[i][j] || k == gridHeight - 1)
                    {
                        scenicScores[i][j] *= k - i;
                        break;
                    }
                }
            }
        }

        return scenicScores.SelectMany(x => x).Max();
    }
}