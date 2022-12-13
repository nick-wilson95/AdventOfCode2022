using AdventOfCode2022.Solutions;
using AdventOfCode2022.Solutions.Days;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace AdventOfCode2022.Tests;

public class DayTests
{
    [Fact]
    public void ConfirmResults()
    {
        using (new AssertionScope())
        {
            ConfirmResult<Day1>("206582");
            ConfirmResult<Day2>("12111");
            ConfirmResult<Day3>("2479");
            ConfirmResult<Day4>("911");
            ConfirmResult<Day5>("QNDWLMGNS");
            ConfirmResult<Day6>("2334");
            ConfirmResult<Day7>("5756764");
            ConfirmResult<Day8>("201684");
            ConfirmResult<Day9>("2376");
            ConfirmResultFromFile<Day10>("day10_expected");
            ConfirmResult<Day11>("54832778815");
            ConfirmResult<Day12>("480");
            ConfirmResult<Day13>("21922");
        }
    }

    private static void ConfirmResult<T>(string expected) where T : Day, new() =>
        new T().Solve()
            .Should()
            .Be(expected, $"that is correct for {typeof(T).Name}");

    private static void ConfirmResultFromFile<T>(string fileName) where T : Day, new() =>
        ConfirmResult<T>(File.ReadAllText($"Expected/{fileName}.txt").Replace("\r\n", "\n"));
}