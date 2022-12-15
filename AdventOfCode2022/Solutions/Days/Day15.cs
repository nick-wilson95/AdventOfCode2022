namespace AdventOfCode2022.Solutions.Days;

public record SensorData((int x, int y) Sensor, (int x, int y) ClosestBeacon)
{
    public int DistanceFromClosest = Math.Abs(Sensor.x - ClosestBeacon.x) + Math.Abs(Sensor.y - ClosestBeacon.y);
    public int DistanceFromRow2Mil = Math.Abs(Sensor.y - 2_000_000);
}

public class Day15 : Day<IEnumerable<SensorData>>
{
    protected override string InputFileName => "day15";
    
    protected override IEnumerable<SensorData> Parse(IEnumerable<string> input)
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
        });
    }

    protected override object Solve(IEnumerable<SensorData> input)
    {
        var beaconlessSections = input
            .Where(x => x.DistanceFromRow2Mil <= x.DistanceFromClosest)
            .Select(data =>
            {
                var difference = data.DistanceFromClosest - data.DistanceFromRow2Mil;
                return (data.Sensor.x - difference, data.Sensor.x + difference);
            });

        var beaconsOnRow2Mil = input.Select(x => x.ClosestBeacon)
            .Distinct()
            .Count(x => x.y == 2_000_000);

        return beaconlessSections.SelectMany(x => Enumerable.Range(x.Item1, x.Item2 - x.Item1 + 1))
            .Distinct()
            .Count() - beaconsOnRow2Mil;
    }
}