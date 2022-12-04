namespace AdventOfCode2022.Extensions;

public static class RangeExtensions
{
    public static bool Overlaps(this Range r1, Range r2) =>
        !(r1.End.Value < r2.Start.Value || r1.Start.Value > r2.End.Value);
}