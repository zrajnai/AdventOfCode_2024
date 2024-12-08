var lines = File.ReadAllLines("Day2.txt");
var sum = lines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse).ToArray())
    .Select(values => IsSafe(values) ? 1 : 0)
    .Sum();

Console.WriteLine(sum);

return;

static bool IsSafe(ICollection<int> values)
{
    var isSafe = IsSafeCore(values);

    if (isSafe) return isSafe;

    for (var i = 0; i < values.Count; i++)
    {
        var valuesCopy = new List<int>(values);
        valuesCopy.RemoveAt(i);
        if (IsSafeCore(valuesCopy))
            return true;
    }

    return isSafe;
}

static bool IsSafeCore(IEnumerable<int> values)
{
    int? previousValue = null;
    var increasing = false;
    var decreasing = false;

    foreach (var value in values)
    {
        if (previousValue.HasValue)
        {
            switch (value - previousValue)
            {
                case >= 1 and <= 3: increasing = true; break;
                case <= -1 and >= -3: decreasing = true; break;
                default:
                    increasing = false; decreasing = false; break;
            }

            if (increasing == decreasing)
            {
                return false;
            }
        }
        previousValue = value;
    }

    return increasing ^ decreasing;
}
