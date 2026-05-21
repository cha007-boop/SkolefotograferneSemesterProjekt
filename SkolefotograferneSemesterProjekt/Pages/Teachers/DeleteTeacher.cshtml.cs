using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class DeleteTeacherModel : PageModel
    {
        private ITeacherService _teacherService;

        public Teacher? TeacherToDelete { get; set; }

        public DeleteTeacherModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        public async Task<IActionResult> OnGet(int? id)
        {
            if(id != null)
            {
                TeacherToDelete = await _teacherService.GetByID((int)id);
                if (TeacherToDelete == null)
                {
                    return RedirectToPage("/Teachers/Index");
                }
                else
                {
                    return Page();
                } 
            }
            return RedirectToPage("/Users/AccessDenied");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            int? role = HttpContext.Session.GetInt32("Role");
            if (userID.HasValue && userID == id || role.HasValue && role == (int)UserRole.SysAdmin)
            {
                TeacherToDelete = await _teacherService.GetByID(id);
                if (TeacherToDelete == null)
                {
                    return RedirectToPage("Index");
                }
                try
                {
                    await _teacherService.Delete(TeacherToDelete);
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return Page();
                }
                return RedirectToPage("Index");
            }
            else
            {
                return RedirectToPage("/Users/AccessDenied");
            }
        }
    }
}
