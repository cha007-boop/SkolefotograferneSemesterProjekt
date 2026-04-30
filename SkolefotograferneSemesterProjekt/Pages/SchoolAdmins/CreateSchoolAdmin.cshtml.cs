using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolAdmins
{
    public class CreateSchoolAdminModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;
        private ISchoolService _schoolService;

        [BindProperty]
        public SchoolAdmin NewSchoolAdmin { get; set; }

        public IEnumerable<SelectListItem> Schools { get; set; }
        public CreateSchoolAdminModel(ISchoolAdminService schoolAdminService, ISchoolService schoolService)
        {
            _schoolAdminService = schoolAdminService;
            _schoolService = schoolService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

            ModelState.Clear();
            TryValidateModel(NewSchoolAdmin);

            try
            {
                await _schoolAdminService.Add(NewSchoolAdmin);
            }
            catch (TakenMailException Tex)
            {
                ModelState.AddModelError("NewSchoolAdmin.Email", Tex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("GetAllSchoolAdmin");
        }
    }
}
