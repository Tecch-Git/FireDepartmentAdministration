using FDA.Backend.Application.Models;
using FDA.Backend.Application.Services;
using FDA.Database.Model;
using System.Text;

namespace FDA.Backend.Functions
{
    public class CSVProcessing
    {
        public static IEnumerable<MessageResponse> LoadMessagesFromCSV()
        {
            string path = "C:\\git\\FireDepartmentAdministration\\Testdata\\TestcsvSimple.csv";
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
            string currentGroup = string.Empty;

            foreach (var line in File.ReadLines(path))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(';');
                string gruppe = parts[0].Trim();
                string person = parts.Length > 1 ? parts[1].Trim() : string.Empty;

                if (!string.IsNullOrEmpty(gruppe))
                {
                    currentGroup = gruppe;
                    if (!groups.ContainsKey(currentGroup))
                        groups[currentGroup] = new List<string>();
                }

                if (!string.IsNullOrEmpty(currentGroup) && !string.IsNullOrEmpty(person))
                {
                    groups[currentGroup].Add(person);
                }
            }

            foreach (var kvp in groups)
            {
                string gruppe = kvp.Key;
                List<string> personen = kvp.Value;

                Console.WriteLine($"Gruppe: {gruppe}");
                foreach (var person in personen)
                {
                    Console.WriteLine($"  - {person}");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            List<MessageResponse> result = new List<MessageResponse>();
            foreach (var kvp in groups)
            {
                string group = kvp.Key;
                List<string> persons = kvp.Value;

                Console.WriteLine($"Gruppe: {group}");
                foreach (var person in persons)
                {
                    var newMessage = new MessageResponse
                    {
                        TargetNumber = "+436643531554",
                        Message = $"Hallo {person}, du bist heuer eingeteilt für die {group}"
                    };
                    result.Add(newMessage);
                    Console.WriteLine($"Hallo {person}, du bist heuer eingeteilt für die {group}");
                }
                Console.WriteLine();
            }

            return result;
        }

        public List<Member> LoadMembersFromCSV(IFormFile file)
        {
            var result = new List<Member>();
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    // CSV with two columns: Name, Membership
                    if (values.Length >= 2)
                    {
                        var name = values[0];
                        if (name == null || name == string.Empty)
                            continue;

                        if (result.Select(m => m.Name).ToList().Contains(name))
                        {
                            Console.WriteLine($"Double value!! Name: {name}");
                            continue;
                        }

                        MEMBERSHIP membership = MEMBERSHIP.None;
                        var membershipString = values[1];
                        if (membershipString == "Aktiv")
                            membership = MEMBERSHIP.Active;
                        else if (membershipString == "Reserve")
                            membership = MEMBERSHIP.Reserve;
                        else if (membershipString == "Jugend")
                            membership = MEMBERSHIP.Youth;

                        result.Add(new Member
                        {
                            Name = name,
                            Membership = membership
                        });
                    }
                }
            }
            return result;
        }

        public List<Member> LoadNumbersFromCSV(IFormFile file)
        {
            var result = new List<Member>();
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');

                    // CSV with two columns: Name, Phonenumber
                    if (values.Length >= 2)
                    {
                        var name = values[0];
                        if (name == null || name == string.Empty)
                            continue;

                        if (result.Select(m => m.Name).ToList().Contains(name))
                        {
                            Console.WriteLine($"Double value!! Name: {name}");
                            continue;
                        }

                        result.Add(new Member
                        {
                            Name = name,
                            Phone = values[1]
                        });
                    }
                }
            }
            return result;
        }
    }
}
