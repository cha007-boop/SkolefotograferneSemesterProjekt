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
        }

        public async Task<IActionResult> OnGet(int id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");

            if (userID.HasValue && Role.HasValue)
            {
                TheTeacher = await _teacherService.GetByID(id);
                if (TheTeacher != null)
                {
                    ClassList = await _schoolClassService.GetAllByTeacher(TheTeacher.ID);
                }
                if (Role == (int)UserRole.Teacher)
                {
                    TheTeacher = await _teacherService.GetByID((int)userID);
                    Teacher? t = TheTeacher;
                    if (t != null && t.ID == id)
                    {
                        IsUser = true;
                    }
                }
                else if (Role == (int)UserRole.SchoolAdmin)
                {
                    SchoolAdmin schoolAdmin = await _schoolAdminService.GetById((int)userID);
                    int schoolID = schoolAdmin.TheSchool.ID;
                    if(schoolID != TheTeacher.TheSchool.ID)
                    {
                        RedirectToPage("/Users/AccessDenied");
                    }
                }
            }
            else
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            return Page();
        }
    }
}
