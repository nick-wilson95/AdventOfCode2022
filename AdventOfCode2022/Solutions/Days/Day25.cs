namespace AdventOfCode2022.Solutions.Days;

public class Day25 : Day<List<string>>
{
    protected override string InputFileName => "day25";

    protected override List<string> Parse(IEnumerable<string> input) => input.ToList();

    protected override object Solve(List<string> input)
    {
        var totals = new Dictionary<int, int>();

        input.ForEach(line =>
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (!totals.ContainsKey(i)) totals[i] = 0;
                totals[i] += line[line.Length - i - 1] switch
                {
                    '=' => -2,
                    '-' => -1,
                    '0' => 0,
                    '1' => 1,
                    '2' => 2
                };
            }
        });

        var result = string.Empty;

        for (var i = 0; i <= totals.Keys.Max(); i++)
        {
            if (!totals.ContainsKey(i)) totals[i] = 0;

            var total = totals[i];

            var remainder = (total + 2).Mod(5) - 2;

            result = remainder switch
            {
                -2 => '=',
                -1 => '-',
                0 => '0',
                1 => '1',
                2 => '2'
            } + result;

            totals[i] = remainder;

            var carry = (total - remainder) / 5;

            if (carry == 0) continue;

            if (!totals.ContainsKey(i + 1)) totals[i + 1] = 0;

            totals[i + 1] += carry;
        }

        return result;
    }
}
