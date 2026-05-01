using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Schools
{
    public class CreateSchoolModel : PageModel
    {
        private ISchoolService _schoolService;

        [BindProperty] 
        public School NewSchool { get; set; }
        public CreateSchoolModel(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            int? userRole = HttpContext.Session.GetInt32("Role");
            if (userId == null || userRole == null)
            {
                return RedirectToPage("/Users/Login");
            }
            if ((UserRole)userRole != UserRole.SysAdmin)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _schoolService.Add(NewSchool);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("GetAllSchool");
        }
    }
}
