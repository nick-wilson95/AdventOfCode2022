namespace AdventOfCode2022.Solutions.Days;

public class Day13 : Day<IEnumerable<(string,string)>>
{
    protected override string InputFileName => "day13";

    protected override IEnumerable<(string, string)> Parse(IEnumerable<string> input) =>
        input.Split(x => x == string.Empty)
            .Select(x => (x[0], x[1]));

    protected override object Solve(IEnumerable<(string, string)> input) =>
        input.Select(x => OrderCorrect(x.Item1, x.Item2) != false)
            .Select((isCorrect, index) => (isCorrect, index))
            .Where(x => x.isCorrect)
            .Sum(x => x.index + 1);

    private static bool? OrderCorrect(string item1, string item2)
    {
        var integerItem1 = int.TryParse(item1, out var value1);
        var integerItem2 = int.TryParse(item2, out var value2);

        if (integerItem1 && integerItem2)
        {
            return (value2 - value1) switch
            {
                < 0 => false,
                > 0 => true,
                0 => null
            };
        }
        if (integerItem1) return OrderCorrect($"[{value1}]", item2);
        if (integerItem2) return OrderCorrect(item1, $"[{value2}]");

        var elements1 = GetElements(item1);
        var elements2 = GetElements(item2);

        for (var i = 0; i < elements1.Count; i++)
        {
            if (i >= elements2.Count) return false;

            var orderCorrect = OrderCorrect(elements1[i], elements2[i]);

            if (orderCorrect.HasValue) return orderCorrect;
        }

        return elements1.Count < elements2.Count ? true : null;
    }

    private static List<string> GetElements(string chars)
    {
        if (chars == "[]") return new List<string>();
        
        var currentDepth = 0;

        var elements = new List<List<char>> { new() };
            
        for (var i = 1; i < chars.Length - 1; i++)
        {
            if (currentDepth == 0 && chars[i] == ',')
            {
                elements.Add(new());
                continue;
            }
                
            if (chars[i] == '[') currentDepth++;
            if (chars[i] == ']') currentDepth--;

            elements.Last().Add(chars[i]);
        }

        return elements.Select(x => new string(x.ToArray())).ToList();
    }
}