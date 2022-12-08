namespace AdventOfCode2022.Utils;

public static class ParseInt
{
    public static int FromChar(char toParse) => (int)char.GetNumericValue(toParse);
}