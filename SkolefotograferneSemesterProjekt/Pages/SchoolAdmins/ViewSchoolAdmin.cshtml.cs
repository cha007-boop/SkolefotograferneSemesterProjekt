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

        public async Task OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            int? userRole = HttpContext.Session.GetInt32("Role");
            if (userId == null || userRole == null)
            {
                RedirectToPage("/Users/Login");
            }
            if ((UserRole)userRole != UserRole.SchoolAdmin)
            {
                RedirectToPage("/Index");
            }
            try
            {
                TheSchoolAdmin = await _schoolAdminService.GetById((int)userId);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
