namespace AdventOfCode2022.Solutions.Days;

public class Day17 : Day<Queue<char>>
{
    protected override string InputFileName => "day17";

    private readonly List<int[,]> _shapes = new()
    {
        new[,] { {1,1,1,1} },
        new[,] { {0,1,0}, {1,1,1}, {0,1,0} },
        new[,] { {0,0,1}, {0,0,1}, {1,1,1} },
        new[,] { {1}, {1}, {1}, {1} },
        new[,] { {1,1}, {1,1} }
    };

    protected override Queue<char> Parse(IEnumerable<string> input) => new(input.Single());

    protected override object Solve(Queue<char> input)
    {
        var jetQueueLength = input.Count;
        var jetsUsed = 0;
        
        var chamber = new List<int[]>();
        
        for (var i = 0;; i++)
        {
            var shapeIndex = i % _shapes.Count;
            var shape = _shapes[shapeIndex];
            Drop(input, chamber, shape, () => jetsUsed++);
        }
        
        // To solve part 2, I realised the tower was repeating every 1740 drops from 1737 drops.
        // 1,000,000,000,000 = 1737 + 574,712,642 * 1740 + 1183
        // So the tower height will be:
        //  The height after block 1737 (2714)
        //  + the 574,712,642 * the repeating section height (2724)
        //  + the extra height after a further 1183 blocks (1860)
        //  = 1,565,517,241,382
    }

    private static void Drop(Queue<char> jets, List<int[]> chamber, int[,] shape, Action onJetUse)
    {
        (int x, int y) pos = (2, chamber.Count + 3);

        while (true)
        {
            var jet = jets.Dequeue();
            onJetUse();
            
            jets.Enqueue(jet);
            var newX = jet == '>' ? pos.x + 1 : pos.x - 1;
            var newPos = (newX, pos.y);

            if (IsValid(chamber, shape, newPos))
            {
                pos = newPos;
            }

            newPos = (pos.x, pos.y - 1);

            if (IsValid(chamber, shape, newPos))
            {
                pos = newPos;
                continue;
            }

            ComeToRest(chamber, shape, pos);
            break;
        }
    }

    private static bool IsValid(List<int[]> chamber, int[,] shape, (int x, int y) pos)
    {
        var width = shape.GetLength(1);
        var height = shape.GetLength(0);
        
        if (pos.x < 0 ) return false;
        if (pos.x + width - 1 > 6) return false;
        if (pos.y < 0) return false;
        
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
            if (shape[j, i] == 0) continue;

            var absoluteX = pos.x + i;
            var absoluteY = pos.y + height - j - 1;

            if (absoluteY > chamber.Count - 1) continue;
            if (chamber[absoluteY][absoluteX] == 1) return false;
        }

        return true;
    }

    private static void ComeToRest(List<int[]> chamber, int[,] shape, (int x, int y) pos)
    {
        var width = shape.GetLength(1);
        var height = shape.GetLength(0);

        var rowsToAdd = pos.y + height - chamber.Count;

        if (rowsToAdd > 0)
        {
            for (var i = 0; i < rowsToAdd; i++) chamber.Add(new int[7]);
        }
        
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
            if (shape[j, i] == 0) continue;

            var absoluteX = pos.x + i;
            var absoluteY = pos.y + height - j - 1;

            chamber[absoluteY][absoluteX] = 1;
        }
    }
}