using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class CreateParentModel : PageModel
    {
        private IParentServices _parentservices;
        private IWebHostEnvironment _webHost;

        [BindProperty]
        public Parent NewParent { get; set; }

        public CreateParentModel(IParentServices parentService, IWebHostEnvironment webHost)
        {
            _parentservices = parentService;
            _webHost = webHost;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (NewParent.Consent == true)
                {
                    await _parentservices.AddParent(NewParent);
                }
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return RedirectToPage("Pages/Index");

        }
    }
}
