using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolAdmins
{
    public class ViewSchoolAdminModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;

        [BindProperty]
        public SchoolAdmin TheSchoolAdmin { get; set; }

        public ViewSchoolAdminModel(ISchoolAdminService schoolAdminService)
        {
            _schoolAdminService = schoolAdminService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            if (HttpContext.Session.GetInt32("Role") != 4)
            {
                throw new UnauthorizedAccessException();
            }
            try
            {
                TheSchoolAdmin = await _schoolAdminService.GetById(id);
            }
            catch (UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccessDenied");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return Page();
        }
    }
}
