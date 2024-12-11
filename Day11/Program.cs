using Item = (ulong value, ulong count);

var input = File.ReadAllText("Day11.txt");
var stones = input
    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(ulong.Parse)
    .GroupBy(v => v)
    .Select(g => (g.Key, (ulong)g.Count()));

CountStones(stones, 25);
CountStones(stones, 75);
return;

void CountStones(IEnumerable<Item> valueTuples, int iterations)
{
    var count = Enumerable
        .Range(0, iterations)
        .Aggregate(valueTuples, (s, _) => GetNextIteration(s.Compact()))
        .Aggregate(0UL, (a, item) => a + item.count);
    Console.WriteLine(count);
}

IEnumerable<Item> GetNextIteration(IEnumerable<Item> stones)
{
    using var it = stones.GetEnumerator();
    while (it.MoveNext())
    {
        var value = it.Current.value;
        var itemCount = it.Current.count;
        var valueStr = value.ToString();
        if (value == 0)
        {
            yield return (1, itemCount);
        }
        else if (valueStr.Length % 2 == 0)
        {
            yield return (ulong.Parse(valueStr[..(valueStr.Length / 2)]), itemCount);
            yield return (ulong.Parse(valueStr[(valueStr.Length / 2)..]), itemCount);
        }
        else
        {
            yield return (value * 2024, itemCount);
        }
    }
}

internal static class Extensions
{
    public static IEnumerable<Item> Compact(this IEnumerable<Item> items) =>
        items
            .GroupBy(item => item.value)
            .Select(g => (g.Key, g.Aggregate(0UL, (a, it) => a + it.count)));
}
// 199986 OK
// 236804088748754 OK