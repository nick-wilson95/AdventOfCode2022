namespace AdventOfCode2022.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<int> Cumulate(this IEnumerable<int> input)
    {
        var sum = 0;
        
        foreach (var element in input)
        {
            sum += element;
            yield return sum;
        }
    }
}