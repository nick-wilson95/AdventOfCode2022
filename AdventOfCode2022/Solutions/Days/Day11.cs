using System.Numerics;

namespace AdventOfCode2022.Solutions.Days;

public class Monkey
{
    public List<long> Items { get; private set; }
    private Func<long, long> Operation { get; init; }
    public Func<long, int> GetNextMonkey { get; private init; }
    public long NumInspections { get; private set; }
    public long Divisor { get; private init; }

    public void InspectItems(long divisorProduct)
    {
        Items = Items.Select(x => Operation(x) % divisorProduct).ToList();
        NumInspections += Items.Count;
    }

    public static Monkey Parse(List<string> data)
    {
        var operationParts = data[2][23..].Split(' ');
        
        Func<long,long,long> @operator = operationParts.First().Single() == '*'
            ? (x, y) => x * y
            : (x, y) => x + y;

        Func<long, long> operation = operationParts[1] == "old"
            ? x => @operator(x, x)
            : x => @operator(x, long.Parse(operationParts[1]));

        var divisor = long.Parse(data[3][21..]);
        var trueCase = int.Parse(data[4][29..]);
        var falseCase = int.Parse(data[5][30..]);
        
        return new Monkey
        {
            Divisor = divisor,
            Items = data[1].Split(':')[1].Split(',').Select(long.Parse).ToList(),
            Operation = operation,
            GetNextMonkey = x => x % divisor == 0 ? trueCase : falseCase
        };
    }
}

public class Day11 : Day<Monkey[]>
{
    protected override string InputFileName => "day11";
    
    protected override Monkey[] Parse(IEnumerable<string> input) =>
        input.Split(x => x == string.Empty)
            .Select(Monkey.Parse).ToArray();

    protected override object Solve(Monkey[] input)
    {
        var divisorProduct = input.Select(x => x.Divisor)
            .Aggregate((x, y) => x * y);
        
        for (var i = 0; i < 10000; i++)
            foreach (var monkey in input)
            {
                monkey.InspectItems(divisorProduct);
                monkey.Items.ForEach(x => input[monkey.GetNextMonkey(x)].Items.Add(x));
                monkey.Items.Clear();
            }

        return input.Select(x => x.NumInspections)
            .Order()
            .TakeLast(2)
            .Aggregate(BigInteger.One, (x, y) => x * y);
    }
}