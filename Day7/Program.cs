//Part1(ParseExamples(File.ReadAllLines("Day7_1.txt")));

Part2(ParseExamples(File.ReadAllLines("Day7_2.txt")));
return;

void Part1(Example[] examples)
{
    var total = 0L;
    foreach (var example in examples)
    {
        for (var i = 0; i < 1 << example.Operands.Length - 1; i++)
        {
            var result = example.Operands[0];
            for (var j = 0; j < example.Operands.Length - 1; j++)
            {
                if ((i & 1 << j) != 0)
                {
                    result += example.Operands[j + 1];
                }
                else
                {
                    result *= example.Operands[j + 1];
                }
            }
            if (result == example.Result)
            {
                Console.WriteLine($"Found {result} for {string.Join(" ", example.Operands)}");
                total += result;
                break;
            }
        }
    }
    Console.WriteLine(total);
}

void Part2(Example[] examples)
{
    var total = 0L;

    foreach (var example in examples)
    {
        var operations = example.Operands.Length - 1;
        for (var i = 0; i < 3.Pow(operations); i++)
        {
            var result = example.Operands[0];
            for (var j = 0; j < example.Operands.Length - 1; j++)
            {
                var nextOperand = example.Operands[j + 1];
                if (i / 3.Pow(j) % 3 == 0)
                {
                    result += nextOperand;
                }
                else if (i / 3.Pow(j) % 3 == 1)
                {
                    result *= nextOperand;
                }
                else
                {
                    result = long.Parse(result.ToString() + nextOperand.ToString());
                }
            }
            if (result == example.Result)
            {
                Console.WriteLine($"Found {result} for {string.Join(" ", example.Operands)}");
                total += result;
                break;
            }
        }
    }

    Console.WriteLine(total);
}

Example[] ParseExamples(string[] strings)
{
    return strings.Select(line => line.Split(":"))
        .Select(parts => new Example
        {
            Result = long.Parse(parts[0]),
            Operands = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()
        }).ToArray();
}

internal static class IntExtensions
{
    public static int Pow(this int bas, int exp)
    {
        return Enumerable
            .Repeat(bas, exp)
            .Aggregate(1, (a, b) => a * b);
    }
}

internal struct Example
{
    public long Result;
    public long[] Operands;
}