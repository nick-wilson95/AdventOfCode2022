namespace AdventOfCode2022.Solutions.Days;

public class Monkey
{
    public List<int> Items { get; private set; }
    private Func<int, int> Operation { get; init; }
    public Func<int, int> GetNextMonkey { get; private init; }
    public int NumInspections { get; private set; }

    public void InspectItems()
    {
        Items = Items.Select(x => Operation(x) / 3).ToList();
        NumInspections += Items.Count;
    }

    public static Monkey Parse(List<string> data)
    {
        var operationParts = data[2][23..].Split(' ');
        
        Func<int,int,int> @operator = operationParts.First().Single() == '*'
            ? (x, y) => x * y
            : (x, y) => x + y;

        Func<int, int> operation = operationParts[1] == "old"
            ? x => @operator(x, x)
            : x => @operator(x, int.Parse(operationParts[1]));

        var divisor = int.Parse(data[3][21..]);
        var trueCase = int.Parse(data[4][29..]);
        var falseCase = int.Parse(data[5][30..]);
        
        return new Monkey
        {
            Items = data[1].Split(':')[1].Split(',').Select(int.Parse).ToList(),
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
        for (var i = 0; i < 20; i++)
            foreach (var monkey in input)
            {
                monkey.InspectItems();
                monkey.Items.ForEach(x => input[monkey.GetNextMonkey(x)].Items.Add(x));
                monkey.Items.Clear();
            }

        return input.Select(x => x.NumInspections)
            .Order()
            .TakeLast(2)
            .Aggregate((x, y) => x * y);
    }
}