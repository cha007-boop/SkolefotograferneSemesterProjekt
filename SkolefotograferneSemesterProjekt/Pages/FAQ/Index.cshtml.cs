using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.FAQ
{
    public class IndexModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;

        public List<string> Entries { get; set; } = [];
        public bool IsAdmin { get; set; }
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
                Entries = await FAQHelper.FAQReader(_webHostEnvironment.WebRootPath, FolderName, FileName, Entries);
                return Page();
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
        }
    }
}
