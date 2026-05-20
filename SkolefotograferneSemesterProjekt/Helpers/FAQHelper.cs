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
                arr = arr.Append("Ingen entry").ToArray();
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
        public async static Task FAQWriter(string webRootPath, string folderName, string fileName, string[] arr)
        {
            string? filePath = Path.Combine(webRootPath, folderName, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                int i = 0;
                while (i < arr.Length)
                {
                    if (i == arr.Length-1)
                    {
                        await writer.WriteLineAsync(arr[i]);
                        return;
                    }
                    await writer.WriteLineAsync(arr[i] + "|");
                    i++;
                }
            }
        }
    }
}
