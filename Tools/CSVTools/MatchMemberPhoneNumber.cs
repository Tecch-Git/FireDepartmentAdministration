using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSVTools;

/// <summary>
/// Input: two paths of
/// </summary>
public class MatchMemberPhoneNumber
{
    public static void Run(string membersPath, string numbersPath, string outputPath)
    {
        var personsCsvRaw = File.ReadAllLines(membersPath, Encoding.UTF8)
            .Select(CleanName)
            .Distinct()
            .ToList();

        var numberEntries = File.ReadAllLines(numbersPath, Encoding.UTF8)
            .Select(line =>
            {
                var parts = line.Split(',');
                return parts.Length >= 2
                    ? (Name: CleanName(parts[0]), Phone: parts[1].Trim())
                    : (Name: "", Phone: "");
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Name))
            .Distinct()
            .ToList();

        var nameToPhone = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var unmatchedPersons = new List<string>();

        int inputCnt = personsCsvRaw.Count;
        int numbersCnt = numberEntries.Count;
        int matchErrCnt = 0;
        int numErrCnt = numberEntries.Count(e => string.IsNullOrEmpty(e.Phone));
        foreach (var personName in personsCsvRaw)
        {
            var matchedEntrie = numberEntries.FirstOrDefault(entry =>
                entry.Name.Equals(SwapNameParts(personName, true), StringComparison.OrdinalIgnoreCase) ||
                entry.Name.Equals(SwapNameParts(personName, false), StringComparison.OrdinalIgnoreCase) ||
                entry.Name.Equals(SwapNameParts(personName, false, true), StringComparison.OrdinalIgnoreCase) ||
                entry.Name.Equals(SwapNameParts(personName, true, true), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(matchedEntrie.Name))
            {
                nameToPhone[personName] = matchedEntrie.Phone;
            }
            else
            {
                if (personName == "Seiwald Lisa")
                {
                    Console.WriteLine("test");
                }
                unmatchedPersons.Add(personName);
                matchErrCnt++;
            }
        }

        var outputLines = new List<string>();
        foreach (var name in personsCsvRaw)
        {
            var phone = nameToPhone.TryGetValue(name, out var p) ? p : "";
            outputLines.Add($"{name},{phone}");
        }

        File.WriteAllLines(outputPath, outputLines, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(
            $"{inputCnt} Persons input\n" +
            $"{numbersCnt} Numbers input\n" +
            $"{inputCnt - numbersCnt} diff (persons-Number)\n" +
            $"{matchErrCnt} Not matched.");
        Console.WriteLine($"{numErrCnt} numbers missing");


        Console.WriteLine("----------------------------");
        foreach (var person in unmatchedPersons)
        {
            Console.WriteLine(person);
        }
    }

    static string CleanName(string rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
            return "";

        // Remove birthyear like "(2010)"
        string withoutYear = Regex.Replace(rawName, @"\(\d{4}\)", "");
        // Remove useless escapes
        string normalized = Regex.Replace(withoutYear, @"\s+", " ").Trim();
        return normalized;
    }

    static string SwapNameParts(string fullName, bool startsWithSurname, bool removeSecondFirstName = true)
    {
        var parts = fullName.Split(' ');
        if (parts.Length < 2) return fullName;

        if (removeSecondFirstName && parts.Length == 3)
            parts = [parts[0], parts[2]];

        if (startsWithSurname)
            return string.Join(" ", parts[1..].Prepend(parts[0]));
        else
            return string.Join(" ", parts[..^1].Prepend(parts[^1]));
    }
}
