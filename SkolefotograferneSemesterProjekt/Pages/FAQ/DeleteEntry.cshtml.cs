using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.FAQ
{
    public class DeleteEntryModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public List<string> Entries { get; set; } = [];
        [BindProperty]
        public int EntryID { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }
        //[BindProperty]
        private string FileName { get; set; }
        private string FolderName { get; set; }

        public DeleteEntryModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            FolderName = "faq";
            FileName = "FAQtekst.txt";
        }
        public async Task<IActionResult> OnGet(int? id)
        {
            if (!id.HasValue)
            {
                ModelState.AddModelError("EntryID", "Ugyldig entry...");
                return RedirectToPage("/Users/AccessDenied");
            }
            EntryID = id.Value;

            int? role = HttpContext.Session.GetInt32("Role");
            if (role != (int)UserRole.SysAdmin)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            int? role = HttpContext.Session.GetInt32("Role");
            if (role != (int)UserRole.SysAdmin)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            try
            {
                Entries = await FAQHelper.FAQReader(_webHostEnvironment.WebRootPath, FolderName, FileName, Entries);
                Entries.RemoveAt(EntryID);

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
