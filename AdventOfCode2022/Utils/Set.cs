namespace AdventOfCode2022.Utils;

public static class Set
{
    public static HashSet<T> Intersection<T>(params HashSet<T>[] sets) =>
        sets.Aggregate(sets[0], (x, y) => x.Intersect(y).ToHashSet());
}