using Array = AdventOfCode2022.Utils.Array;
using static AdventOfCode2022.Solutions.Days.Day22.Direction;

namespace AdventOfCode2022.Solutions.Days;

public class Day22 : Day<(char[][,] map, int[] distances, char[] turns)>
{
    private const int CubeSize = 50;

    public enum Direction { Right, Down, Left, Up }

    protected override string InputFileName => "day22";

    private record Position(Direction direction, int face, int x, int y) { }

    protected override (char[][,] map, int[] distances, char[] turns) Parse(IEnumerable<string> input)
    {
        var parts = input.Split(x => x == string.Empty);

        var mapInput = parts[0];
        var commandInput = parts[1].Single();

        var mapWidth = mapInput.Max(x => x.Length);
        var mapOld = mapInput.Select(x =>
        {
            var endSpace = Array.Of(_ => ' ', mapWidth - x.Length);
            return x.Concat(endSpace).ToArray();
        }).ToArray();

        var distances = commandInput
            .Split('R', 'L')
            .Select(int.Parse)
            .ToArray();

        var turns = commandInput
            .Where(x => x == 'R' || x == 'L')
            .ToArray();

        var map = input.Chunk(CubeSize).SelectMany(chunk =>
        {
            var sectionedLines = chunk.Select(line => line.SkipWhile(x => x != '.' && x != '#').Chunk(CubeSize).ToArray())
                .ToArray();

            var numFaces = sectionedLines.First().Count();

            var faces = new char[numFaces][,];

            for (var faceIndex = 0; faceIndex < numFaces; faceIndex++)
            {
                var face = new char[CubeSize, CubeSize];

                for (var i = 0; i < CubeSize; i++)
                for (var j = 0; j < CubeSize; j++)
                {
                        face[i, j] = sectionedLines[j][faceIndex][i];
                }

                faces[faceIndex] = face;
            }

            return faces;
        }).ToArray();

        return (map, distances, turns);
    }

    protected override object Solve((char[][,] map, int[] distances, char[] turns) input)
    {
        var (map, distances, turns) = input;

        Position pos = new Position(Right, 0, 0, 0);

        while(true)
        {
            pos = GetStepPosition(pos, map);
            if (map[pos.face][pos.x, pos.y] == '.') break;
        }

        Direction Turn(Direction direction, char side) =>
            (Direction)(((int)direction + (side == 'L' ? 3 : 1)) % 4);

        pos = Move(pos, distances[0], map);

        for (var i = 0; i < turns.Length; i++)
        {
            pos = pos with { direction = Turn(pos.direction, turns[i]) };
            pos = Move(pos, distances[i + 1], map);
        }

        (int x, int y) facePosition = pos.face switch
        {
            0 => (50,0),
            1 => (100, 0),
            2 => (50, 50),
            3 => (0, 100),
            4 => (50, 100),
            5 => (0, 150),
        };

        return 1000 * (facePosition.y + pos.y + 1) + 4 * (facePosition.x + pos.x + 1) + (int)pos.direction;
    }

    private static Position Move(Position from, int distance, char[][,] map)
    {
        var stepPos = from;
        var newPos = from;
        var steps = 0;

        while (true)
        {
            if (steps == distance) break;

            stepPos = GetStepPosition(stepPos, map);
            var stepChar = map[stepPos.face][stepPos.x, stepPos.y];

            if (stepChar == '#') break;

            if (stepChar == '.')
            {
                steps++;
                newPos = stepPos;
            }
        }

        return newPos;
    }

    private static Position GetStepPosition(Position from, char[][,] map)
    {
        (int x, int y) nextPos = from.direction switch
        {
            Right => (from.x + 1, from.y),
            Down => (from.x, from.y + 1),
            Left => (from.x - 1, from.y),
            Up => (from.x, from.y - 1),
        };

        if (nextPos.x < 0 || nextPos.x > CubeSize - 1 || nextPos.y < 0 || nextPos.y > CubeSize - 1)
        {
            return GetCorneredPosition(from);
        }

        return @from with { x = nextPos.x, y = nextPos.y};
    }

    private static Position GetCorneredPosition(Position from)
    {
        var max = CubeSize - 1;

        (int face, Direction direction) newFaceAndDirection = (from.face, from.direction) switch
        {
            (0, Right) => (1, Right),
            (0, Down) => (2, Down),
            (0, Left) => (3, Right),
            (0, Up) => (5, Right),

            (1, Right) => (4, Left),
            (1, Down) => (2, Left),
            (1, Left) => (0, Left),
            (1, Up) => (5, Up),

            (2, Right) => (1, Up),
            (2, Down) => (4, Down),
            (2, Left) => (3, Down),
            (2, Up) => (0, Up),

            (3, Right) => (4, Right),
            (3, Down) => (5, Down),
            (3, Left) => (0, Right),
            (3, Up) => (2, Right),

            (4, Right) => (1, Left),
            (4, Down) => (5, Left),
            (4, Left) => (3, Left),
            (4, Up) => (2, Up),

            (5, Right) => (4, Up),
            (5, Down) => (1, Down),
            (5, Left) => (0, Down),
            (5, Up) => (3, Up),
        };

        (int x, int y) newXY = (from.direction, newFaceAndDirection.direction) switch
        {
            (Right, Right) => (0, from.y),
            (Right, Down) => (max - from.y, 0),
            (Right, Left) => (max, max - from.y),
            (Right, Up) => (from.y, max),

            (Down, Right) => (0, max - from.x),
            (Down, Down) => (from.x, 0),
            (Down, Left) => (max, from.x),
            (Down, Up) => (max - from.x, max),

            (Left, Right) => (0, max - from.y),
            (Left, Down) => (from.y, 0),
            (Left, Left) => (max, from.y),
            (Left, Up) => (max - from.y, max),

            (Up, Right) => (0, from.x),
            (Up, Down) => (max - from.x, 0),
            (Up, Left) => (max, max - from.x),
            (Up, Up) => (from.x, max),
        };

        return new Position(newFaceAndDirection.direction, newFaceAndDirection.face, newXY.x, newXY.y);
    }
}
