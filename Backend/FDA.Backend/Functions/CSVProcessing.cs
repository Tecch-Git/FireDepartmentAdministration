using FDA.Backend.Application.Models;

namespace FDA.Backend.Functions
{
    public class CSVProcessing
    {
        string path = "C:\\git\\FireDepartmentAdministration\\Testdata\\TestcsvSimple.csv";

        Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
        string currentGroup = string.Empty;

        public IEnumerable<MessageResponse> LoadMessagesFromCSV()
        {
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
    }
}
