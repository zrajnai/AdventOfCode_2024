var input = File.ReadAllText("Day9.txt");

//var entries = CreateEntries(input);
//CompactEntries1(entries);
//Console.WriteLine(CalculateSum(entries));

var entries2 = CreateEntries(input).Where(e => e.Size > 0).ToList();
CompactEntries2(entries2);
Console.WriteLine(CalculateSum(entries2));
return;

IList<Entry> CreateEntries(string input)
{
    var entries = new List<Entry>();
    short id = 0;
    for (var i = 0; i < input.Length;)
    {
        entries.Add(new FileEntry { ID = id++, Size = short.Parse(input[i++].ToString()) });
        if (i == input.Length) break;

        entries.Add(new SpaceEntry { Size = short.Parse(input[i++].ToString()) });
    }

    return entries;
}

void CompactEntries1(IList<Entry> entries)
{
    var spaceIndex = 0;
    var fileIndex = entries.Count;

    while (spaceIndex < fileIndex)
    {
        while (entries[++spaceIndex] is not SpaceEntry && spaceIndex < fileIndex) { }

        if (spaceIndex >= fileIndex) break;
        var spaceEntry = (SpaceEntry)entries[spaceIndex];

        while (entries[--fileIndex] is not FileEntry && fileIndex > 0) { }
        if (spaceIndex >= fileIndex) break;
        var fileEntry = (FileEntry)entries[fileIndex];

        while (fileEntry.Size > 0)
        {
            if (spaceEntry.Size < fileEntry.Size)
            { // not enough empty space
                fileEntry.Size -= spaceEntry.Size;
                entries[spaceIndex] = new FileEntry { ID = fileEntry.ID, Size = spaceEntry.Size };

                while (entries[++spaceIndex] is not SpaceEntry && spaceIndex < fileIndex) { }
                if (spaceIndex >= fileIndex) break;
                spaceEntry = (SpaceEntry)entries[spaceIndex];
            }
            else
            { // space is enough
                entries[spaceIndex] = new FileEntry { ID = fileEntry.ID, Size = fileEntry.Size };
                var spaceLeft = (short)(spaceEntry.Size - fileEntry.Size);
                if (spaceLeft > 0)
                {
                    spaceEntry = new SpaceEntry { Size = spaceLeft };
                    entries.Insert(spaceIndex + 1, spaceEntry);
                    fileIndex++;
                }
                entries.RemoveAt(fileIndex);
                break;
            }
        }
    }

}

void CompactEntries2(IList<Entry> entries)
{
    var fileIndex = entries.Count;
    while (fileIndex > 0)
    {
        // Get the next file entry from the end
        while (entries[--fileIndex] is not FileEntry && fileIndex >= 0) { }
        if (fileIndex < 0) break;
        var fileEntry = (FileEntry)entries[fileIndex];

        var spaceIndex = 0;
        SpaceEntry? spaceEntry = null;
        do
        {
            while (entries[++spaceIndex] is not SpaceEntry && spaceIndex < fileIndex) { }
            if (spaceIndex >= fileIndex) break;
            spaceEntry = (SpaceEntry)entries[spaceIndex];
        } while (spaceEntry.Size < fileEntry.Size);

        if (spaceEntry?.Size >= fileEntry.Size)
        {
            entries[spaceIndex] = new FileEntry { ID = fileEntry.ID, Size = fileEntry.Size };
            if (spaceEntry.Size > fileEntry.Size)
            {
                entries.Insert(spaceIndex + 1, new SpaceEntry { Size = (short)(spaceEntry.Size - fileEntry.Size) });
                fileIndex++;
            }

            entries[fileIndex] = new SpaceEntry { Size = fileEntry.Size };
        }
    }
}

long CalculateSum(IEnumerable<Entry> entries)
{
    long sum = 0;
    long position = 0;
    foreach (var entry in entries)
    {
        for (var i = 0; i < entry.Size; i++)
        {
            if (entry is FileEntry fileEntry)
            {
                sum += position * fileEntry.ID;
            }
            position++;
        }
    }
    return sum;
}

internal class Entry
{
    public short Size;
};

internal class FileEntry : Entry
{
    public short ID;
}

internal class SpaceEntry : Entry;
