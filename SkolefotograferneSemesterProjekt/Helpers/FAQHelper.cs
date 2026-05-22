using Microsoft.AspNetCore.Hosting;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Helpers
{
    public static class FAQHelper
    {
        public async static Task<List<FAQEntry>> FAQReader(string webRootPath, string folderName, string fileName)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);
            
            List<FAQEntry> list = [];
            if (!System.IO.File.Exists(filePath))
            {
                return list;
            }

            string temp = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(temp))
            {
                return list;
            }

            string[] entries = temp.Split("|");

            foreach (string entry in entries)
            {
                string[] parts = entry.Split("::");

                if (parts.Length == 2)
                {
                    list.Add(new FAQEntry
                    {
                        Question = parts[0],
                        Answer = parts[1]
                    });
                }
            }
            return list;
        }
        public async static Task FAQWriter(string webRootPath, string folderName, string fileName, List<FAQEntry> list)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            List<string> entries = [];
            foreach (FAQEntry entry in list)
            {
                entries.Add(entry.Question + "::" + entry.Answer);
            }

            string temp = string.Join("|", entries);
            await File.WriteAllTextAsync(filePath, temp);
        }
    }
}
