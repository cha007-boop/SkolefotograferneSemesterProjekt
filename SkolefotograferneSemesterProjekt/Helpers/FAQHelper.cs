using Microsoft.AspNetCore.Hosting;

namespace SkolefotograferneSemesterProjekt.Helpers
{
    public static class FAQHelper
    {
        
        public async static Task<string[]> FAQReader(string webRootPath, string folderName, string fileName, string[] arr)
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
            arr = temp.Split("|");
            return arr;
        }
        public async static Task FAQWriter(string webRootPath, string folderName, string fileName, string[] arr)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            string temp = string.Join("|", arr);
            await File.WriteAllTextAsync(filePath, temp);
        }
    }
}
