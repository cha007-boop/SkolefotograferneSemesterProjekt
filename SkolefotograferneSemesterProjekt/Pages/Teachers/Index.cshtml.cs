using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers.Filter;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        public ITeacherService _teacherService;
        public ISchoolAdminService _schoolAdminService;
        public List<Teacher> TeacherList { get; set; }
        public Teacher TheTeacher { get; set; }
        public int? UserID { get; set; }
        public bool IsUser { get; set; }
        [BindProperty]
        public int? Role { get; set; }
        [BindProperty]
        public IEnumerable<Teacher> TeacherFList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }

        public IndexModel(ITeacherService teacherService, ISchoolAdminService schoolAdminService)
        {
            _teacherService = teacherService;
            _schoolAdminService = schoolAdminService;
        }

        public async Task<IActionResult> OnGet()
        {
            UserID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");

            if (UserID.HasValue && Role.HasValue)
            {
                if (Role == (int)UserRole.Teacher)
                {
                    TeacherList = await _teacherService.GetAll();

                    Teacher t = new Teacher { ID = (int)UserID };
                    t = TeacherList.Find(t => t.ID == UserID);
                    if (t != null)
                    {
                        IsUser = true;
                        TheTeacher = t;
                    }
                }
                else if(Role == (int)UserRole.SchoolAdmin)
                {
                    SchoolAdmin schoolAdmin = await _schoolAdminService.GetById((int)UserID);
                    int schoolID = schoolAdmin.TheSchool.ID;
                    TeacherList = await _teacherService.GetBySchoolID(schoolID);
                }
                else if(Role == (int)UserRole.SysAdmin)
                {
                    TeacherList = await _teacherService.GetAll();
                }
                else
                {
                    return RedirectToPage("/Users/AccessDenied");
                }
                TeacherFList = Filter(TeacherList);
            }
            else
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            return Page();
        }
        private IEnumerable<Teacher> Filter(IEnumerable<Teacher> tLst)
        {
            List<Predicate<Teacher>> predicates = new List<Predicate<Teacher>>();
            if(Role == (int)UserRole.SysAdmin)
            {
                if (!string.IsNullOrWhiteSpace(FilterCriteria))
                {
                    switch (FilterBy)
                    {
                        case "t.TheSchool.Name":
                            predicates.Add(t => !string.IsNullOrEmpty(t.TheSchool.Name) && t.TheSchool.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                            break;
                        case "t.Email":
                            predicates.Add(t => !string.IsNullOrEmpty(t.Email) && t.Email.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                            break;
                        default:
                            break;
                    }
                }
            }
            if(Role == (int)UserRole.SchoolAdmin)
            {
                if (!string.IsNullOrWhiteSpace(FilterCriteria))
                {
                    switch (FilterBy)
                    {
                        case "t.FirstName":
                            predicates.Add(t => !string.IsNullOrEmpty(t.FirstName) && t.FirstName.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                            break;
                        case "t.Email":
                            predicates.Add(t => !string.IsNullOrEmpty(t.Email) && t.Email.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                            break;
                        default:
                            break;
                    }
                }
            }
            return FilterFunctions<Teacher>.Filter(tLst, predicates);
        }
    }
}
