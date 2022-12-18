namespace AdventOfCode2022.Extensions;

public static class TupleExtensions
{
    public static void ForAllAdjacent(this (int x, int y, int z) p, Action<(int x, int y, int z)> action)
    {
        var adjacent = new[]
        {
            (p.x + 1, p.y, p.z),
            (p.x - 1, p.y, p.z),
            (p.x, p.y + 1, p.z),
            (p.x, p.y - 1, p.z),
            (p.x, p.y, p.z + 1),
            (p.x, p.y, p.z - 1)
        };

        foreach (var q in adjacent)
        {
            action(q);
        }
    }
}