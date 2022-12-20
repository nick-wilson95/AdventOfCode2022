namespace AdventOfCode2022.Solutions.Days;

public class Node { public int Value { get; init; } }

public class Day20 : Day<Node[]>
{
    protected override string InputFileName => "day20";

    protected override Node[] Parse(IEnumerable<string> input) =>
        input.Select(x => new Node { Value = int.Parse(x) }).ToArray();

    protected override object Solve(Node[] input)
    {
        var previous = input.Zip(input.Skip(1)).ToDictionary(x => x.Second, x => x.First);
        previous[input[0]] = input[^1];
        previous[input[^1]] = input[^2];

        var next = input.Zip(input.Skip(1)).ToDictionary(x => x.First, x => x.Second);
        next[input[^2]] = input[^1];
        next[input[^1]] = input[0];

        void SwitchWithNext(Node n1)
        {
            var n0 = previous[n1];
            var n2 = next[n1];
            var n3 = next[n2];

            next[n0] = n2;

            previous[n1] = n2;
            next[n1] = n3;

            previous[n2] = n0;
            next[n2] = n1;

            previous[n3] = n1;
        }

        Node zeroNode = null;

        foreach (var node in input)
        {
            if (node.Value > 0)
            {
                for (var i = 0; i < node.Value; i++)
                {
                    SwitchWithNext(node);
                }
            }
            else if (node.Value < 0)
            {
                for (var i = 0; i < -node.Value; i++)
                {
                    SwitchWithNext(previous[node]);
                }
            }
            else zeroNode = node;
        }

        var currentNode = zeroNode;
        var sum = 0;

        for (var i = 1; i <= 3000; i++)
        {
            currentNode = next[currentNode];
            if (i % 1000 == 0) sum += currentNode.Value;
        }

        return sum;
    }
}
