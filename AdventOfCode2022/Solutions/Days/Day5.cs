using AdventOfCode2022.Extensions;
using Array = AdventOfCode2022.Utils.Array;

namespace AdventOfCode2022.Solutions.Days;

public record StackInstructions(Stack<char>[] Stacks,
    List<(int repeats, int fromIndex, int toIndex)> Instructions) {}

public class Day5 : Day<StackInstructions>
{
    protected override string InputFileName => "day5";

    protected override StackInstructions Parse(string[] input)
    {
        var inputParts = input.Split(x => x == string.Empty);

        return new StackInstructions(
            ParseStacks(inputParts[0]),
            ParseInstructions(inputParts[1])
        );
    }

    protected override object Solve(StackInstructions input)
    {
        input.Instructions.ForEach(x =>
        {
            for (var i = 0; i < x.repeats; i++)
            {
                var popped = input.Stacks[x.fromIndex - 1].Pop();
                input.Stacks[x.toIndex - 1].Push(popped);
            }
        });

        var topLayer = input.Stacks.Select(x => x.Peek());

        return string.Concat(topLayer);
    }

    private static Stack<char>[] ParseStacks(IEnumerable<string> input)
    {
        var numStacks = input.Last()
            .Split("   ")
            .Select(int.Parse)
            .Last();

        IEnumerable<char> GetColumn(int i) =>
            input.Reverse()
                .Skip(1)
                .Select(x => x[i])
                .Where(x => x is not ' ');

        return Array.Of(
            i => new Stack<char>(GetColumn(i * 4 + 1)),
            numStacks);
    }

    private static List<(int, int, int)> ParseInstructions(IEnumerable<string> input) =>
        input.Select(x => x.Split(' '))
            .Select(x => (int.Parse(x[1]), int.Parse(x[3]), int.Parse(x[5])))
            .ToList();
}