namespace AdventOfCode2022.Solutions.Days;

public class Directory
{
    public required Directory? Parent { get; init; }
    public Dictionary<string, int> Files { get; } = new();
    public Dictionary<string, Directory> Children { get; } = new();
}

public class Day7 : Day<Directory>
{
    protected override string InputFileName => "day7";
    
    protected override Directory Parse(IEnumerable<string> input)
    {
        var rootNode = new Directory{ Parent = null };
        var currentNode = rootNode;

        foreach (var line in input)
        {
            if (line is "$ ls") continue;
            if (line is "$ cd /") currentNode = rootNode;
            else if (line is "$ cd ..") currentNode = currentNode.Parent;
            else if (line is ['$', ' ', 'c', 'd', ' ', .. var to]) currentNode = currentNode.Children[to];
            else if (line is ['d', 'i', 'r', ' ', .. var dir]) currentNode.Children.Add(dir, new Directory{ Parent = currentNode });
            else
            {
                var lineParts = line.Split(' ');
                currentNode.Files.Add(lineParts[1], int.Parse(lineParts[0]));
            }
        }

        return rootNode;
    }

    protected override object Solve(Directory input)
    {
        var sizes = new List<int>();
        
        int RecursivelyCheckSizes(Directory dir)
        {
            var size = dir.Files.Values.Sum() + dir.Children.Sum(x => RecursivelyCheckSizes(x.Value));
            sizes.Add(size);
            return size;
        }

        var totalSize = RecursivelyCheckSizes(input);

        return sizes.Order().First(x => 40_000_000 >= totalSize - x);
    }
}