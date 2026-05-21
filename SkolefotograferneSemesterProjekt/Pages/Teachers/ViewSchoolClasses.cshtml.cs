using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class ViewSchoolClassesModel : PageModel
    {
        ITeacherService _teacherService;
        ISchoolClassService _schoolClassService;
        ISchoolAdminService _schoolAdminService;

        public Teacher? TheTeacher { get; set; }
        public List<SchoolClass> ClassList { get; set; }
        public bool IsUser { get; set; }
        public int? Role { get; set; }

        public ViewSchoolClassesModel(ITeacherService teacherService, ISchoolClassService schoolClassService, ISchoolAdminService schoolAdminService)
        {
            _teacherService = teacherService;
            _schoolClassService = schoolClassService;
            _schoolAdminService = schoolAdminService;
            ClassList = new List<SchoolClass>();
        }

        public async Task<IActionResult> OnGet()
        {
            int userID = (int)HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");

            if (Role == (int)UserRole.Teacher)
            {                
                ClassList = await _schoolClassService.GetAllByTeacher(userID);
            }
            else if (Role == (int)UserRole.SchoolAdmin)
            {
                SchoolAdmin schoolAdmin = await _schoolAdminService.GetById(userID);
                ClassList = await _schoolClassService.GetBySchool(schoolAdmin.TheSchool.ID);
            }
            else if (HttpContext.Session.GetInt32("Role") == (int)UserRole.SysAdmin)
            {
                ClassList = await _schoolClassService.GetAll();
            }
            else
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                await _schoolClassService.Delete(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            await OnGet();
            return Page();
        }
    }
}
