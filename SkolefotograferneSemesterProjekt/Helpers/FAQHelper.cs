using Microsoft.AspNetCore.Hosting;

namespace SkolefotograferneSemesterProjekt.Helpers
{
    public static class FAQHelper
    {
        
        public async static Task<Array> FAQReader(string webRootPath, string folderName, string fileName, string[] arr)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                arr.Append("Ingen entry");
                return arr;
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
    }
}
