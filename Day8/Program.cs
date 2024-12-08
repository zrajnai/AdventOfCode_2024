using Position = (int x, int y);

var fileMap = File.ReadAllLines("Day8.txt");

var antennas = GetAntennas(fileMap);

var antinodesMap1 = MarkAntinodes1((fileMap.Length, fileMap[0].Length), antennas);
Console.WriteLine($"Number of antinodes: {CountAntinodes(antinodesMap1)}");

var antinodesMap2 = MarkAntinodes2((fileMap.Length, fileMap[0].Length), antennas);
Console.WriteLine($"Number of antinodes: {CountAntinodes(antinodesMap2)}");

return;

Dictionary<char, Position[]> GetAntennas(string[] map)
{
    var antennas = new Dictionary<char, Position[]>();
    var y = 0;
    foreach (var mapLine in map)
    {
        var x = 0;
        foreach (var mapPos in mapLine)
        {
            if (mapPos != '.')
            {
                if (antennas.TryGetValue(mapPos, out var positions))
                {
                    antennas[mapPos] = positions.Append((x, y)).ToArray();
                }
                else
                {
                    antennas[mapPos] = [(x, y)];
                }
            }

            x++;
        }
        y++;
    }
    return antennas;
}

char[,] MarkAntinodes2((int, int) mapSize, Dictionary<char, Position[]> antennas)
{
    var map = new char[mapSize.Item1, mapSize.Item2];

    foreach (var positions in antennas.Select(frequency => frequency.Value))
    {
        for (var i = 0; i < positions.Length; i++)
        {
            for (var j = i + 1; j < positions.Length; j++)
            {
                var pos1 = positions[i];
                var pos2 = positions[j];
                var diff = (x: pos1.x - pos2.x, y: pos1.y - pos2.y);

                while (IsOnMap(map, pos1))
                {
                    map[pos1.y, pos1.x] = '#';
                    pos1 = (x: pos1.x + diff.x, y: pos1.y + diff.y);
                }

                while (IsOnMap(map, pos2))
                {
                    map[pos2.y, pos2.x] = '#';
                    pos2 = (x: pos2.x - diff.x, y: pos2.y - diff.y);
                }
            }
        }
    }

    return map;
}
char[,] MarkAntinodes1((int, int) mapSize, Dictionary<char, Position[]> antennas)
{
    var map = new char[mapSize.Item1, mapSize.Item2];

    foreach (var positions in antennas.Select(frequency => frequency.Value))
    {
        for (var i = 0; i < positions.Length; i++)
        {
            for (var j = i + 1; j < positions.Length; j++)
            {
                var pos1 = positions[i];
                var pos2 = positions[j];
                var diff = (x: pos1.x - pos2.x, y: pos1.y - pos2.y);

                var antiNode1Pos = (x: pos1.x + diff.x, y: pos1.y + diff.y);
                if (IsOnMap(map, antiNode1Pos))
                {
                    map[antiNode1Pos.y, antiNode1Pos.x] = '#';
                }

                var antiNode2Pos = (x: pos2.x - diff.x, y: pos2.y - diff.y);
                if (IsOnMap(map, antiNode2Pos))
                {
                    map[antiNode2Pos.y, antiNode2Pos.x] = '#';
                }
            }
        }
    }

    return map;
}

bool IsOnMap(char[,] map, Position valueTuple)
{
    return valueTuple.x >= 0 &&
           valueTuple.x < map.GetLength(1) &&
           valueTuple.y >= 0 &&
           valueTuple.y < map.GetLength(0);
}

int CountAntinodes(char[,] map)
{
    var count = 0;
    for (var i = 0; i < map.GetLength(0); i++)
    {
        for (var j = 0; j < map.GetLength(1); j++)
        {
            if (map[i, j] == '#')
            {
                count++;
            }
        }
    }

    return count;
}