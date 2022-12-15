using System.Numerics;

namespace AdventOfCode2022.Solutions.Days;

public record SensorData((int x, int y) Sensor, (int x, int y) ClosestBeacon)
{
    public readonly int DistanceFromClosest = Math.Abs(Sensor.x - ClosestBeacon.x) + Math.Abs(Sensor.y - ClosestBeacon.y);
    public int DistanceFromRow(int row) => Math.Abs(Sensor.y - row);
}

public class Day15 : Day<List<SensorData>>
{
    protected override string InputFileName => "day15";
    
    protected override List<SensorData> Parse(IEnumerable<string> input)
    {
        return input.Select(line =>
        {
            var parts = line.Split('=', ',', ':');
            
            var salientParts = new[] { 1, 3, 5, 7 }
                .Select(x => parts[x])
                .Select(int.Parse)
                .ToArray();
            
            return new SensorData(
                (salientParts[0], salientParts[1]),
                (salientParts[2], salientParts[3])
            );
        }).ToList();
    }

    protected override object Solve(List<SensorData> input)
    {
        input = input.OrderByDescending(x => x.DistanceFromClosest).ToList();

        var lastSkipSequence = new List<SensorData>();

        var useSkipSequence = false;

        var y = 0;

        while (true)
        {
            startOuterLoop:
            
            var x = 0;
            
            while (true)
            {
                startInnerLoop:
                
                if (x > 4_000_000)
                {
                    y++;
                    useSkipSequence = true;
                    goto startOuterLoop;
                }

                var useInput = useSkipSequence ? lastSkipSequence : input;

                foreach (var data in useInput)
                {
                    var difference = data.DistanceFromClosest - data.DistanceFromRow(y);
                    
                    if (difference < 0) continue;

                    if (data.Sensor.x - difference <= x && data.Sensor.x + difference >= x)
                    {
                        x = data.Sensor.x + difference + 1;
                        
                        if (!useSkipSequence) lastSkipSequence.Add(data);
                        
                        goto startInnerLoop;
                    }
                }

                if (useSkipSequence)
                {
                    useSkipSequence = false;
                    lastSkipSequence.Clear();
                    goto startOuterLoop;
                }
                
                return new BigInteger(4_000_000) * new BigInteger(x) + new BigInteger(y);
            }
        }
    }
}