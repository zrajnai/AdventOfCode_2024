using System.Text;
var input = File.ReadAllText("Day3.txt");
var sum = 0;
var index = 0;
var enabled = true;
do
{
    if (input[index..].StartsWith("do()"))
    {
        enabled = true;
        index += 4;
        continue;
    }
    if (input[index..].StartsWith("don't()"))
    {
        enabled = false;
        index += 7;
        continue;
    }
    if (input[index..].StartsWith("mul("))
    {
        index += 4;
    }
    else
    {
        index++;
        continue;
    }

    var op1 = ReadDigits();
    if (op1 == null) continue;

    if (input[index++] != ',')
    {
        continue;
    }

    var op2 = ReadDigits();
    if (op2 == null) continue;

    if (input[index++] != ')')
    {
        continue;
    }

    sum += enabled ? op1.Value * op2.Value : 0;
} while (index < input.Length);

Console.WriteLine(sum);
return;

int? ReadDigits()
{
    var digits = new StringBuilder();
    var startIndex = index;
    while (char.IsDigit(input[index]) && index <= startIndex + 2)
    {
        digits.Append(input[index]);
        index++;
    }

    if (digits.Length is > 3 or 0) return null;

    return int.Parse(digits.ToString());
}