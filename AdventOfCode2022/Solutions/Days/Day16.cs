namespace AdventOfCode2022.Solutions.Days;

// See graph http://graphonline.ru/en/?graph=CBKSHhtMsDiAmGgL
public record ValveData(int FlowRate, List<string> Connections);

public class Day16 : Day<Dictionary<string,ValveData>>
{
    protected override string InputFileName => "day16";
    
    protected override Dictionary<string,ValveData> Parse(IEnumerable<string> input)
    {
        var result = new Dictionary<string, ValveData>();
        
        foreach (var line in input)
        {
            var key = line.Substring(6, 2);
            var flowRate = int.Parse(line.Split('=', ';')[1]);

            var lastPart = line.Split("to valve")[1];
            var connectionsStart = lastPart[0] == ' ' ? 1 : 2;
            var connections = lastPart[connectionsStart..].Split(", ").ToList();
            
            result[key] = new ValveData(flowRate, connections);
        }

        return result;
    }

    protected override object Solve(Dictionary<string,ValveData> input)
    {
        var distances = new Dictionary<(string, string), int>();

        foreach (var kvp in input)
        {
            foreach (var connection in kvp.Value.Connections)
            {
                distances[(kvp.Key, connection)] = 1;
            }
        }

        foreach (var kvp in input.Where(x => x.Value.FlowRate == 0 && x.Key != "AA"))
        {
            var connections = kvp.Value.Connections;
            var (first, second) = (connections[0], connections[1]);
            
            var newDistance = distances[(first, kvp.Key)] + distances[(second, kvp.Key)];

            if (!distances.TryGetValue((first, second), out var value) || newDistance < value)
            {
                distances[(first, second)] = newDistance;
                distances[(second, first)] = newDistance;
            }

            input[first].Connections.Remove(kvp.Key);
            input[second].Connections.Remove(kvp.Key);

            input[first].Connections.Add(second);
            input[second].Connections.Add(first);

            distances.Remove((first, kvp.Key));
            distances.Remove((second, kvp.Key));
            distances.Remove((kvp.Key, first));
            distances.Remove((kvp.Key, second));
        }
        var best = 0;

        void PostScore(List<string> path)
        {
            var visited = new HashSet<string>();

            var elapsed = 0;

            var pressureReleased = 0;
            
            for (var i = 1; i < path.Count(); i++)
            {
                elapsed += distances[(path[i - 1], path[i])];

                if (!visited.Contains(path[i]))
                {
                    elapsed++;
                    visited.Add(path[i]);
                    pressureReleased += (30 - elapsed) * input[path[i]].FlowRate;
                }
            }

            if (pressureReleased > best)
            {
                if (pressureReleased == 2114)
                {
                    var a = 1;
                }
                Console.WriteLine($"new best: {pressureReleased}");
                best = pressureReleased;
            }
        }
            
        void Recurse(int elapsed, IEnumerable<string> path)
        {
            var currentValve = path.Last();

            var connections = input[currentValve].Connections;

            var canMove = false;
            
            foreach (var valve in connections)
            {
                var distance = distances[(currentValve, valve)];
                if (elapsed + distance + 1 >= 30) continue;
                canMove = true;

                if (path.Contains(valve))
                {
                    Recurse(elapsed + distance, path.Concat(new[] { valve }));
                }
                else
                {
                    Recurse(elapsed + distance + 1, path.Concat(new[] { valve }));
                }

            }

            if (!canMove) PostScore(path.ToList());
        }

        Recurse(0, new List<string>{"AA"});

        return best;
    }
}