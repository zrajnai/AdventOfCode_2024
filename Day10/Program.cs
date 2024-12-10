using Map = string[];
using Position = (int x, int y);
using Trail = (int x, int y)[];

var input = File.ReadAllLines("Day10.txt");

var startingPoints = FindStartingPoints(input);
var sum1 = 0;
var sum2 = 0;
foreach (var startingPoint in startingPoints)
{
    var trailEnds = new HashSet<(Position, Position)>();
    var trails = new List<Trail>();
    FindTrails(input, startingPoint, trail =>
    {
        trailEnds.Add((trail.First(), trail.Last()));
        trails.Add(trail.ToArray());
    });
    sum1 += trailEnds.Count;
    sum2 += trails.Count;
}

Console.WriteLine($"{sum1}, {sum2}");
return;

Position[] FindStartingPoints(Map map)
{
    var positions = new List<Position>();
    for (var y = 0; y < map.Length; y++)
    {
        var s = map[y];
        for (var x = 0; x < map[y].Length; x++)
        {
            if (s[x] == '0')
            {
                positions.Add((x, y));
            }
        }
    }

    return positions.ToArray();
}

void FindTrails(Map map, Position trailHead, Action<Trail> trailFound)
{
    var directions = new Position[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
    var visited = new HashSet<Position>();
    var queue = new Queue<Trail>();
    queue.Enqueue([trailHead]);
    while (queue.Count > 0)
    {
        var currentTrail = queue.Dequeue();
        var currentPos = currentTrail.Last();
        var currentValue = map[currentPos.y][currentPos.x];
        if (currentValue == '9')
        {
            trailFound(currentTrail);
            continue;
        }
        visited.Add(currentPos);
        foreach (var direction in directions)
        {
            Position nextPos = (currentPos.x + direction.x, currentPos.y + direction.y);
            if (visited.Contains(nextPos) ||
                nextPos.x < 0 || nextPos.x >= map[0].Length ||
                nextPos.y < 0 || nextPos.y >= map.Length)
                continue;

            var nextValue = map[nextPos.y][nextPos.x];
            if (nextValue != currentValue + 1)
                continue;

            queue.Enqueue([.. currentTrail, nextPos]);
        }
    }
}
