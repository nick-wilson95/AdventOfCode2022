namespace AdventOfCode2022.Solutions.Days;

public class Day18 : Day<HashSet<(int x, int y, int z)>>
{
    protected override string InputFileName => "day18";
    
    protected override HashSet<(int x, int y, int z)> Parse(IEnumerable<string> input) =>
        input.Select(x => x.Split(',').Select(int.Parse).ToArray())
            .Select(x => (x[0],x[1],x[2]))
            .ToHashSet();

    protected override object Solve(HashSet<(int x, int y, int z)> input)
    {
        var minX = input.Select(p => p.x).Min();
        var maxX = input.Select(p => p.x).Max();
        var minY = input.Select(p => p.y).Min();
        var maxY = input.Select(p => p.y).Max();
        var minZ = input.Select(p => p.z).Min();
        var maxZ = input.Select(p => p.z).Max();

        var exteriorPoint = (minX - 1, minY - 1, minZ - 1);
        var exterior = new HashSet<(int x, int y, int z)>{ exteriorPoint };
        var lastAdditions = new HashSet<(int x, int y, int z)> { exteriorPoint };

        var externalSurfaceArea = 0;
        
        while (true)
        {
            if (!lastAdditions.Any()) break;
            
            var additions = new HashSet<(int x, int y, int z)>();
            
            foreach (var p in lastAdditions)
            {
                p.ForAllAdjacent(q =>
                {
                    var oob = q.x < minX - 1 || q.x > maxX + 1
                            || q.y < minY - 1 || q.y > maxY + 1
                            || q.z < minZ - 1 || q.z > maxZ + 1;

                    if (input.Contains(q)) externalSurfaceArea++;
                    
                    if (oob || exterior.Contains(q) || input.Contains(q)) return;

                    additions.Add(q);
                    exterior.Add(q);
                });
            }

            lastAdditions = additions;
        }

        return externalSurfaceArea;
    }
}