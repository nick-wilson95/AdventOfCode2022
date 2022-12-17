using System.Numerics;

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

        var scoresForIndices = new Dictionary<(int shapeIndex, int jetIndex), List<(int shapeCount, int score)>>();
        
        for (var i = 0;; i++)
        {
            var shapeIndex = i % _shapes.Count;
            var jetIndex = jetsUsed % jetQueueLength;

            var indexExists = scoresForIndices.TryGetValue((shapeIndex, jetIndex), out var data);
            
            if (indexExists
                && data.Count >= 2
                && chamber.Count - data[^1].score == data[^1].score - data[^2].score)
            {
                var preLoopData = data[^2];
                var loopData = data[^1];
                
                var loopScore = loopData.score - preLoopData.score;
                var loopShapeCount = loopData.shapeCount - preLoopData.shapeCount;

                var totalShapeCount = BigInteger.Parse("1000000000000");
                var numLoops = totalShapeCount / loopShapeCount;

                var postLoopShapeCount = totalShapeCount - numLoops * loopShapeCount - preLoopData.shapeCount;
                var loopBookendScore = scoresForIndices.Values.SelectMany(x => x)
                    .Single(x => x.shapeCount == preLoopData.shapeCount + postLoopShapeCount).score;

                return numLoops * loopScore + loopBookendScore;
            }

            if (indexExists) data.Add((i, chamber.Count));
            else scoresForIndices.Add((shapeIndex, jetIndex), new List<(int shapeCount, int score)>{(i, chamber.Count)});

            var shape = _shapes[shapeIndex];
            Drop(input, chamber, shape, () => jetsUsed++);
        }
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