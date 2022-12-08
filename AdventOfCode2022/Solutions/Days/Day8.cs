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
        for (var j = 1; j < gridWidth - 1; j++)
        {
            for (var n = j - 1;; n--)
            {
                if (input[i][n] >= input[i][j] || n == 0)
                {
                    scenicScores[i][j] *= j - n;
                    break;
                }
            }
            for (var n = j + 1;; n++)
            {
                if (input[i][n] >= input[i][j] || n == gridWidth - 1)
                {
                    scenicScores[i][j] *= n - j;
                    break;
                }
            }
            for (var n = i - 1;; n--)
            {
                if (input[n][j] >= input[i][j] || n == 0)
                {
                    scenicScores[i][j] *= i - n;
                    break;
                }
            }
            for (var n = i + 1;; n++)
            {
                if (input[n][j] >= input[i][j] || n == gridHeight - 1)
                {
                    scenicScores[i][j] *= n - i;
                    break;
                }
            }
        }

        return scenicScores.SelectMany(x => x).Max();
    }
}