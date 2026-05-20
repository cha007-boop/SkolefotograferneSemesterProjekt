using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.FAQ
{
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public List<string> Questions { get; set; } = [];
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

            string? filePath = Path.Combine(_webHostEnvironment.WebRootPath, FolderName, FileName);

            if (!System.IO.File.Exists(filePath))
            {
                Questions.Add("Ingen spørgsmål");
            }
            try
            {
                string temp = "";
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream) 
                    {
                        temp += await reader.ReadLineAsync();
                    }
                }
                Questions = temp.Split("|").ToList();
                return Page();
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
    }
}
