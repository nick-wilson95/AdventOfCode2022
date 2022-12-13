namespace AdventOfCode2022.Solutions.Days;

public class Day13 : Day<IEnumerable<string>>
{
    protected override string InputFileName => "day13";

    protected override IEnumerable<string> Parse(IEnumerable<string> input) =>
        input.Where(x => x != string.Empty);

    protected override object Solve(IEnumerable<string> input)
    {
        var dividers = new[] { "[[2]]", "[[6]]"};
        
        var orderedLines = input
            .Concat(dividers)
            .Order(Comparer<string>.Create(Comparer))
            .ToList();

        return dividers.Select(x => orderedLines.IndexOf(x) + 1)
            .Product();
    }

    // Returns:
    //  -1 if item1 is before item2
    //  1 if item1 is after item2
    //  0 otherwise
    private static int Comparer(string item1, string item2)
    {
        var integerItem1 = int.TryParse(item1, out var value1);
        var integerItem2 = int.TryParse(item2, out var value2);

        if (integerItem1 && integerItem2)
            return Math.Clamp(value1 - value2, -1, 1);
        
        if (integerItem1) return Comparer($"[{value1}]", item2);
        if (integerItem2) return Comparer(item1, $"[{value2}]");

        var elements1 = GetElements(item1);
        var elements2 = GetElements(item2);

        for (var i = 0; i < elements1.Count; i++)
        {
            if (i >= elements2.Count) return 1;

            var comparison = Comparer(elements1[i], elements2[i]);

            if (comparison != 0) return comparison;
        }

        return elements1.Count < elements2.Count ? -1 : 0;
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