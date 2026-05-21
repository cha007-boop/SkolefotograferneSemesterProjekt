using Microsoft.AspNetCore.Hosting;

namespace SkolefotograferneSemesterProjekt.Helpers
{
    public static class FAQHelper
    {
        
        public async static Task<List<string>> FAQReader(string webRootPath, string folderName, string fileName, List<string> list)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return ["Ingen entry"];
            }

            string temp = "";
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    temp += await reader.ReadLineAsync();
                }
            }
            list = temp.Split("|").ToList();
            return list;
        }
        public async static Task FAQWriter(string webRootPath, string folderName, string fileName, List<string> list)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            string temp = string.Join("|", list);
            await File.WriteAllTextAsync(filePath, temp);
        }
    }
}
