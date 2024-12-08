var input = File.ReadAllLines("Day5.txt");

var rules = input.TakeWhile(s => !string.IsNullOrEmpty(s)).Select(s => s.Split('|').Select(int.Parse).ToArray()).Select(n => (n[0], n[1])).ToArray();
var updates = input.SkipWhile(s => !string.IsNullOrEmpty(s)).Skip(1).Select(s => s.Split(',').Select(int.Parse).ToArray()).ToArray();

var sum = updates.Where(update => CheckRules(update, rules)).Sum(update => update[update.Length / 2]);
Console.WriteLine(sum);

sum = 0;
foreach (var update in updates)
{
    var isOk = false;
    var neededCorrection = false;
    while (!isOk)
    {
        foreach (var rule in rules)
        {
            isOk = CheckRule(update, rule);
            if (!isOk)
            {
                neededCorrection = true;
                SwapElements(update, rule.Item1, rule.Item2);
                break;
            }
        }
    }

    if (neededCorrection)
    {
        sum += update[update.Length / 2];
    }
}
Console.WriteLine(sum);
return;

static bool CheckRule(int[] update, (int, int) rule)
{
    if (!update.Contains(rule.Item1) || !update.Contains(rule.Item2)) return true;
    return Array.IndexOf(update, rule.Item1) < Array.IndexOf(update, rule.Item2);
}

static bool CheckRules(int[] update, IEnumerable<(int, int)> rules)
{
    return rules.Select(rule => CheckRule(update, rule)).All(isOk => isOk);
}

static void SwapElements(int[] array, int element1, int element2)
{
    var pos1 = Array.IndexOf(array, element1);
    var pos2 = Array.IndexOf(array, element2);

    (array[pos1], array[pos2]) = (array[pos2], array[pos1]);
}

