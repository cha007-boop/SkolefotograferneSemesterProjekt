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

            if (Role == 2)
            {
                TheTeacher = await _teacherService.GetByID(userID);
                if (TheTeacher != null)
                {
                    //int teacherID = id.Value;
                    ClassList = await _schoolClassService.GetAllByTeacher(TheTeacher.ID);
                    if (Role == (int)UserRole.Teacher)
                    {
                        //IsUser = userID == teacherID;
                    }
                    else if (Role == (int)UserRole.SchoolAdmin)
                    {
                        SchoolAdmin schoolAdmin = await _schoolAdminService.GetById(userID);
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
            else if(HttpContext.Session.GetInt32("Role") == 4)
            {
                foreach (SchoolClass sc in await _schoolClassService.GetAll())
                {
                    ClassList.Add(sc);
                }
                return Page();
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
