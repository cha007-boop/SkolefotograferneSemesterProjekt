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

        public async Task<IActionResult> OnGet(int? id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");

            if (id.HasValue)
            {
                if (!userID.HasValue && !Role.HasValue)
                {
                    return RedirectToPage("/Users/AccessDenied");
                }
                TheTeacher = await _teacherService.GetByID(id.Value);
                if (TheTeacher != null)
                {
                    int teacherID = id.Value;
                    ClassList = await _schoolClassService.GetAllByTeacher(TheTeacher.ID);
                    if (Role == (int)UserRole.Teacher)
                    {
                        IsUser = userID == teacherID;
                    }
                    else if (Role == (int)UserRole.SchoolAdmin)
                    {
                        SchoolAdmin schoolAdmin = await _schoolAdminService.GetById(userID.Value);
                        int schoolID = schoolAdmin.TheSchool.ID;
                        int? tSchoolID = TheTeacher.TheSchool.ID;
                        if (tSchoolID.HasValue && schoolID != tSchoolID)
                        {
                            return RedirectToPage("/Users/AccessDenied");
                        }
                    }
                }
                else
                {
                    return RedirectToPage("/Users/AccessDenied");
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
