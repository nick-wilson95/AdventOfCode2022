namespace AdventOfCode2022.Solutions.Days;

public class Day6 : Day<string>
{
    private const int MarkerSize = 14;
    
    protected override string InputFileName => "day6";

    protected override string Parse(IEnumerable<string> input) => input.Single();
    
    protected override object Solve(string input)
    {
        var currentSubstr = new List<char>(input[..(MarkerSize - 1)]);

        var earliestPossibleMarker = 0;

        for (var i = (MarkerSize - 1); i < input.Length; i++)
        for (var j = 1; j < MarkerSize; j++)
        {
            if (currentSubstr[j - 1] == input[i])
            {
                earliestPossibleMarker = Math.Max(earliestPossibleMarker, i + j);
            }

            if (earliestPossibleMarker < i + 1) return i + 1;
            
            currentSubstr.RemoveAt(0);
            currentSubstr.Add(input[i]);
        }

        throw new Exception();
    }
}