using Position = (int x, int y);

var map = File.ReadAllLines("Day6.txt");

var dirMap = new Direction[map.Length, map[0].Length];

var yPos = map.Select((line, index) => new { line, index }).First(x => x.line.Contains('^')).index;
var xPos = map[yPos].IndexOf('^');

// part 1
TraverseMap((x: xPos, y: yPos), Direction.Up, MarkPosition, (pos, _) => IsOutsideMap(pos));
Console.WriteLine(map.Sum(line => line.Count(c => c == 'X')));

// part 2
var obstructions = new HashSet<Position>();
map = File.ReadAllLines("Day6.txt");
TraverseMap((x: xPos, y: yPos), Direction.Up, MarkAndFindLoop, (pos, _) => IsOutsideMap(pos));
Console.WriteLine(obstructions.Count);

return;

void TraverseMap(Position currentPos, Direction currentDirection,
    Action<Position, Direction> mark,
    Func<Position, Direction, bool> shouldTerminate)
{
    while (true)
    {
        mark(currentPos, currentDirection);

        var nextPos = GetNextPos(currentPos, currentDirection);
        if (shouldTerminate(nextPos, currentDirection))
        {
            break;
        }
        if (map[nextPos.y][nextPos.x] == '#' ||
            map[nextPos.y][nextPos.x] == 'O')
        {
            currentDirection = TurnRight(currentDirection);
            continue;
        }

        currentPos = nextPos;
    }
}

void MarkDirection(Position pos, Direction direction)
{
    dirMap[pos.y, pos.x] |= direction;
    UpdateMapWithChar(pos, GetDirectionChar(dirMap[pos.y, pos.x]));
}

void MarkPosition(Position pos, Direction _)
{
    UpdateMapWithChar(pos, 'X');
}

static Position GetOffset(Direction direction) => direction switch
{
    Direction.Up => (0, -1),
    Direction.Down => (0, 1),
    Direction.Left => (-1, 0),
    Direction.Right => (1, 0),
    _ => throw new ArgumentException("Invalid direction")
};

static Position GetNextPos(Position currentPos, Direction direction) =>
   (currentPos.x + GetOffset(direction).x, currentPos.y + GetOffset(direction).y);

static Direction TurnRight(Direction currentDirection) =>
   currentDirection switch
   {
       Direction.Up => Direction.Right,
       Direction.Right => Direction.Down,
       Direction.Down => Direction.Left,
       Direction.Left => Direction.Up,
       _ => throw new ArgumentException("Invalid direction")
   };

static char GetDirectionChar(Direction direction) => direction switch
{
    Direction.Up => '^',
    Direction.Down => 'v',
    Direction.Left => '<',
    Direction.Right => '>',
    Direction.Down | Direction.Up => '|',
    Direction.Left | Direction.Right => '-',
    Direction.Right | Direction.Up => '+',
    Direction.Right | Direction.Down => '+',
    Direction.Left | Direction.Up => '+',
    Direction.Left | Direction.Down => '+',
    _ => '*'
};

bool IsOutsideMap(Position pos)
{
    return pos.y < 0 || pos.y >= map.Length ||
           pos.x < 0 || pos.x >= map[pos.y].Length;
}

void MarkAndFindLoop(Position currPos, Direction currDir)
{
    MarkDirection(currPos, currDir);

    var obsPos = GetNextPos(currPos, currDir);
    if (IsOutsideMap(obsPos) ||
        map[obsPos.y][obsPos.x] != '.')
        return;

    var map2 = (string[])map.Clone();
    var dirMap2 = (Direction[,])dirMap.Clone();

    UpdateMapWithChar(obsPos, 'O');

    var hasLoop = false;
    TraverseMap(currPos, currDir, MarkDirection, (nextPos, nextDir) =>
    {
        if (IsOutsideMap(nextPos))
            return true;
        hasLoop = dirMap[nextPos.y, nextPos.x].HasFlag(nextDir);
        return hasLoop;
    });

    if (hasLoop)
    {
        obstructions.Add(obsPos);
    }

    map = map2;
    dirMap = dirMap2;
}

void UpdateMapWithChar(Position pos, char character)
{
    var line = map[pos.y].ToCharArray();
    line[pos.x] = character;
    map[pos.y] = new string(line);
}

[Flags]
internal enum Direction
{
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8
}
