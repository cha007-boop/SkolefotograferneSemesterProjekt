using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.FAQ
{
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public string[] Entries { get; set; } = [];
        [BindProperty]
        public bool IsAdmin { get; set; }
        //[BindProperty]
        private string FileName { get; set; }
        private string FolderName { get; set; }

        public IndexModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            FolderName = "faq";
            FileName = "FAQtekst.txt";
        }

        public async Task<IActionResult> OnGet()
        {
            int? role = HttpContext.Session.GetInt32("Role");
            if(role == (int)UserRole.SysAdmin)
            {
                IsAdmin = true;
            }

            try
            {
                Entries = (string[])await FAQHelper.FAQReader(_webHostEnvironment.WebRootPath, FolderName, FileName, Entries);
                return Page();
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
        //OG
        //public async Task<IActionResult> OnGet()
        //{
        //    int? role = HttpContext.Session.GetInt32("Role");
        //    if(role == (int)UserRole.SysAdmin)
        //    {
        //        IsAdmin = true;
        //    }

        //    string? filePath = Path.Combine(_webHostEnvironment.WebRootPath, FolderName, FileName);

        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        Questions.Append("Ingen spørgsmål");
        //    }
        //    try
        //    {
        //        string temp = "";
        //        using (StreamReader reader = new StreamReader(filePath))
        //        {
        //            while (!reader.EndOfStream) 
        //            {
        //                temp += await reader.ReadLineAsync();
        //            }
        //        }
        //        Questions = temp.Split("|");
        //        return Page();
        //    }
        //    catch(Exception ex)
        //    {
        //        ViewData["ErrorMessage"] = ex.Message;
        //        return Page();
        //    }
        //}
    }
}
