namespace AdventOfCode2022.Extensions;

public static class StackExtensions
{
    public static void ShiftSubstack<T>(this Stack<T> from, int count, Stack<T> to)
    {
        var temp = new Stack<T>();
        for (var i = 0; i < count; i++) { temp.Push(from.Pop()); }
        for (var i = 0; i < count; i++) { to.Push(temp.Pop()); }
    }
}