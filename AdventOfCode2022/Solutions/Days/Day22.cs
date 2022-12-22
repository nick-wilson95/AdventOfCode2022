using Array = AdventOfCode2022.Utils.Array;

namespace AdventOfCode2022.Solutions.Days;

public class Day22 : Day<(char[][] map, int[] distances, char[] turns)>
{
    protected override string InputFileName => "day22";

    protected override (char[][] map, int[] distances, char[] turns) Parse(IEnumerable<string> input)
    {
        var parts = input.Split(x => x == string.Empty);

        var mapInput = parts[0];
        var commandInput = parts[1].Single();

        var mapWidth = mapInput.Max(x => x.Length);
        var map = mapInput.Select(x =>
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

        return (map, distances, turns);
    }

    protected override object Solve((char[][] map, int[] distances, char[] turns) input)
    {
        var (map, distances, turns) = input;

        var direction = Direction.Right;

        (int x, int y) position = default;
        for (var i = 0; i < map[0].Length; i++)
        {
            if (map[0][i] == '.')
            {
                position = (i, 0);
                break;
            };
        }

        Direction Turn(char side) =>
            (Direction)(((int)direction + (side == 'L' ? 3 : 1)) % 4);

        position = Move(position, direction, distances[0], map);

        for (var i = 0; i < turns.Length; i++)
        {
            direction = Turn(turns[i]);
            position = Move(position, direction, distances[i + 1], map);
        }

        return 1000 * (position.y + 1) + 4 * (position.x + 1) + (int)direction;
    }

    private enum Direction { Right, Down, Left, Up }

    private (int x, int y) Move((int x, int y) from, Direction direction, int distance, char[][] map)
    {
        var nextPosition = from;
        var newPosition = from;
        var steps = 0;

        while (true)
        {
            if (steps == distance) break;

            nextPosition = GetStepPosition(nextPosition, direction, map);
            var nextChar = map[nextPosition.y][nextPosition.x];

            if (nextChar == '#') break;

            if (nextChar == '.')
            {
                steps++;
                newPosition = nextPosition;
            }
        }

        return newPosition;
    }

    private (int x, int y) GetStepPosition((int x, int y) from, Direction direction, char[][] map)
    {
        var width = map[0].Length;
        var length = map.Length;

        (int x, int y) nextPosition = direction switch
        {
            Direction.Right => (from.x + 1, from.y),
            Direction.Down => (from.x, from.y + 1),
            Direction.Left => (from.x - 1, from.y),
            Direction.Up => (from.x, from.y - 1),
        };

        return (
            (nextPosition.x + width) % width,
            (nextPosition.y + length) % length
        );
    }
}
