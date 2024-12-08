var lines = File.ReadAllLines("Day1.txt");

var values1 = new List<int>(lines.Length);
var values2 = new List<int>(lines.Length);

foreach (var line in lines)
{
    var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    values1.Add(int.Parse(values[0]));
    values2.Add(int.Parse(values[1]));
}

values1.Sort();
values2.Sort();
var sum = values1.Zip(values2).Sum(pair => Math.Abs(pair.First - pair.Second));
Console.WriteLine(sum);

var totalSimilarityScore = values1.Sum(value1 => value1 * values2.Count(v => v == value1));
Console.WriteLine(totalSimilarityScore);