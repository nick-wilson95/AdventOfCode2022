namespace AdventOfCode2022.Extensions;

public static class IntExtensions
{
    public static int Mod(this int input, int @base)
    {
        var result = input % @base;
        return result < 0 ? result + @base : result;
    }
}