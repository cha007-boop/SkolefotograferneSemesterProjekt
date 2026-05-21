using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.FAQ
{
    public class AddEntryModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public List<string> Entries { get; set; } = [];
        [BindProperty]
        public string NewEntry { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }
        private string FileName { get; set; }
        private string FolderName { get; set; }

        public AddEntryModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            FolderName = "faq";
            FileName = "FAQtekst.txt";
        }
        public async Task<IActionResult> OnGet()
        {
            int? role = HttpContext.Session.GetInt32("Role");
            if (role != (int)UserRole.SysAdmin)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.CustomizedMessages("Felter Mangler");

            int? role = HttpContext.Session.GetInt32("Role");
            if (role != (int)UserRole.SysAdmin)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            if (string.IsNullOrWhiteSpace(NewEntry))
            {
                return Page();
            }
            try
            {
                Entries = await FAQHelper.FAQReader(_webHostEnvironment.WebRootPath, FolderName, FileName, Entries);
                Entries.Add(NewEntry);

                await FAQHelper.FAQWriter(_webHostEnvironment.WebRootPath, FolderName, FileName, Entries);
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
    }
}
