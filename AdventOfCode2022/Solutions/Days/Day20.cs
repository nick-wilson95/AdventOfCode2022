using System.Numerics;

namespace AdventOfCode2022.Solutions.Days;

public class Node { public BigInteger Value { get; init; } }

public class Day20 : Day<Node[]>
{
    protected override string InputFileName => "day20";

    protected override Node[] Parse(IEnumerable<string> input) =>
        input.Select(x => new Node { Value = int.Parse(x) * new BigInteger(811589153) })
        .ToArray();

    protected override object Solve(Node[] input)
    {
        var previous = input.Zip(input.Skip(1)).ToDictionary(x => x.Second, x => x.First);
        previous[input[0]] = input[^1];
        previous[input[^1]] = input[^2];

        var next = input.Zip(input.Skip(1)).ToDictionary(x => x.First, x => x.Second);
        next[input[^2]] = input[^1];
        next[input[^1]] = input[0];

        Node zeroNode = null;

        for (var repeat = 1; repeat <= 10; repeat++)
        {
            foreach (var node in input)
            {
                if (node.Value == 0)
                {
                    zeroNode = node;
                    continue;
                }

                var (oldPrev, oldNext) = (previous[node], next[node]);
                previous[oldNext] = oldPrev;
                next[oldPrev] = oldNext;

                var newNext = oldNext;
                for (var i = 0; i < BigInteger.Abs(node.Value) % (input.Count() - 1); i++)
                {
                    newNext = node.Value > 0 ? next[newNext] : previous[newNext];
                }
                var newPrev = previous[newNext];

                next[newPrev] = node;
                previous[node] = previous[newNext];
                next[node] = newNext;
                previous[newNext] = node;
            }
        }

        var currentNode = zeroNode;
        BigInteger sum = 0;

        for (var i = 1; i <= 3000; i++)
        {
            currentNode = next[currentNode];
            if (i % 1000 == 0) sum += currentNode.Value;
        }

        return sum;
    }
}
