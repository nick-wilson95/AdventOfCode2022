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
        
        input.Select(x => x.Value.Connections).ToList().ForEach(x => x.RemoveAll(y => y == "AA"));
        
        var scores = new Dictionary<HashSet<string>, int>(HashSet<string>.CreateSetComparer());
            
        void Recurse(int elapsed, int totalP, int pps, string currentValve, HashSet<string> opened)
        {
            var connections = input[currentValve].Connections;

            if (currentValve != "AA" && !opened.Contains(currentValve))
            {
                Recurse(
                    elapsed + 1,
                    totalP + pps,
                    pps + input[currentValve].FlowRate,
                    currentValve,
                    opened.Concat(new []{currentValve}).ToHashSet());
            }

            var canMove = false;
            
            foreach (var valve in connections)
            {
                var distance = distances[(currentValve, valve)];
                if (elapsed + distance + 1 >= 26) continue;

                Recurse(
                    elapsed + distance,
                    totalP + distance * pps,
                    pps,
                    valve,
                    opened);
                
                canMove = true;

            }

            if (canMove) return;
            
            var total = totalP + (26 - elapsed) * pps;

            if (!scores.TryGetValue(opened, out var best) || best < total)
            {
                scores[opened] = total;
            }
        }

        Recurse(0, 0, 0, "AA", new HashSet<string>());

        var topScore = scores.Values.Max();
        var goodScores = scores.Where(x => x.Value > topScore / 2)
            .ToDictionary(x => x.Key, x => x.Value);

        var best = 0;
        Parallel.ForEach(goodScores, x =>
        {
            var bestForScore = scores.Where(kvp => !kvp.Key.Overlaps(x.Key))
                .Select(kvp => kvp.Value + x.Value)
                .Max();

            if (bestForScore > best) best = bestForScore;
        });

        return best;
    }
}