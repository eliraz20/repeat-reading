static class BudgetReport
{
    public static void Run(string[] args)
    {
        if (args.Length != 2)
            throw new FormatException("Usage: budget study.tsv");
        var readings = Rows(args[1], 5).Select(r => (Name: r[0], Unit: r[1], Value: (double)decimal.Parse(r[2]), Step: (double)Amount(r[3]), Other: (double)Amount(r[4])));
        foreach (var group in readings.GroupBy(v => (v.Name, v.Unit)))
        {
            var first = group.First();
            Require(group.All(r => r.Step == first.Step && r.Other == first.Other), "Instrument terms must agree within a group.");
            var data = group.Select(v => v.Value).ToArray();
            var mean = data.Average();
            var resolution = first.Step / Math.Sqrt(12);
            Console.WriteLine($"{first.Name} [{first.Unit}]: n={data.Length}, mean {mean:G6}, resolution u={resolution:G6}, other independent u={first.Other:G6}");
            if (data.Length == 1)
            {
                Console.WriteLine("  repeatability unavailable; combined standard uncertainty undefined");
                continue;
            }

            var repeat = Math.Sqrt(data.Sum(x => (x - mean) * (x - mean)) / (data.Length * (double)(data.Length - 1)));
            Console.WriteLine($"  repeatability u(mean)={repeat:G6}; combined standard uncertainty {Math.Sqrt(repeat * repeat + resolution * resolution + first.Other * first.Other):G6} {first.Unit}");
        }
    }

    static List<string[]> Rows(string file, int width = 0, bool allowEmpty = false)
    {
        using var reader = file == "-" ? Console.In : File.OpenText(file);
        var rows = new List<string[]>();
        int line = 0;
        while (reader.ReadLine()is string s)
        {
            line++;
            if (string.IsNullOrWhiteSpace(s) || s.TrimStart().StartsWith("#"))
                continue;
            var cells = s.Split('\t', StringSplitOptions.TrimEntries);
            if ((width > 0 && cells.Length != width) || (!allowEmpty && cells.Any(string.IsNullOrEmpty)))
                throw new FormatException($"Invalid columns on line {line}.");
            rows.Add(cells);
        }

        if (rows.Count == 0)
            throw new FormatException("Empty input.");
        return rows;
    }

    static void Require(bool condition, string message)
    {
        if (!condition)
            throw new FormatException(message);
    }

    static decimal Amount(string text)
    {
        var value = decimal.Parse(text, System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint);
        Require(value >= 0, "Quantity must be nonnegative.");
        return value;
    }
}
