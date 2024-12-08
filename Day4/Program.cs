var lines = File.ReadAllLines("Day4.txt");
var columns = lines.First().Length;

// find the number of times the word XMAS appears in any direction in the grid represented by the lines
const string xmas = "XMAS";
const string xmasReversed = "SAMX";
var count = 0;

for (var i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < columns; j++)
    {
        if (j <= columns - xmas.Length)
        {
            if (lines[i][j..].StartsWith(xmas) ||
             lines[i][j..].StartsWith(xmasReversed))
            {
                count++;
            }
        }


        if (i <= lines.Length - xmas.Length)
        {
            var down = string.Join("", lines.Skip(i).Take(xmas.Length).Select(x => x[j]));
            if (down is xmas or xmasReversed)
            {
                count++;
            }

            if (j <= columns - xmas.Length)
            {
                var diagonal = string.Join("", lines.Skip(i).Take(xmas.Length).Select((x, k) => x[j + k]));
                if (diagonal is xmas or xmasReversed)
                {
                    count++;
                }
            }

            if (j >= xmas.Length - 1)
            {
                var diagonal = string.Join("", lines.Skip(i).Take(xmas.Length).Select((x, k) => x[j - k]));
                if (diagonal is xmas or xmasReversed)
                {
                    count++;
                }
            }
        }
    }
}

Console.WriteLine(count);

count = 0;
for (var i = 1; i < lines.Length - 1; i++)
{
    for (var j = 1; j < columns - 1; j++)
    {
        if (lines[i][j] != 'A') continue;

        if ((lines[i - 1][j - 1] == 'M' && lines[i + 1][j + 1] == 'S' ||
             lines[i - 1][j - 1] == 'S' && lines[i + 1][j + 1] == 'M') &&
            (lines[i - 1][j + 1] == 'S' && lines[i + 1][j - 1] == 'M' ||
             lines[i - 1][j + 1] == 'M' && lines[i + 1][j - 1] == 'S'))
        {
            count++;
        }
    }
}
Console.WriteLine(count);