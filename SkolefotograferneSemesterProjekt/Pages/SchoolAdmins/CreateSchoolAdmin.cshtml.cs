using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolAdmins
{
    public class CreateSchoolAdminModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;

        [BindProperty]
        public SchoolAdmin NewSchoolAdmin { get; set; }
        public CreateSchoolAdminModel(ISchoolAdminService schoolAdminService)
        {
            _schoolAdminService = schoolAdminService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

            try
            {
                await _schoolAdminService.Add(NewSchoolAdmin);
            }
            catch
            {

            }
            return RedirectToPage("Index");
        }
    }
}
