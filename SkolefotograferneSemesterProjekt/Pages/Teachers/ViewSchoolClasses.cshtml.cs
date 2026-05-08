using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class ViewSchoolClassesModel : PageModel
    {
        ITeacherService _teacherService;
        ISchoolClassService _schoolClassService;

        public Teacher? TheTeacher { get; set; }
        public List<SchoolClass> ClassList { get; set; }
        public int? UserID { get; set; }
        public bool IsUser { get; set; }
        public int? Role { get; set; }

        public ViewSchoolClassesModel(ITeacherService teacherService, ISchoolClassService schoolClassService)
        {
            _teacherService = teacherService;
            _schoolClassService = schoolClassService;
        }

        public async Task OnGet(int id)
        {
            UserID = HttpContext.Session.GetInt32("ID");

            TheTeacher = await _teacherService.GetByID(id);
            if (TheTeacher != null)
            {
                ClassList = await _schoolClassService.GetAllByTeacher(TheTeacher.ID);
            }

            if (UserID != null)
            {
                Teacher t = TheTeacher;
                if (t != null && t.ID == id)
                {
                    IsUser = true;
                }
            }
            Role = HttpContext.Session.GetInt32("Role") ?? -1;
        }
    }
}
