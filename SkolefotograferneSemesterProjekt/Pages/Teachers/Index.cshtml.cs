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
        public ITeacherService _repo;
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

        public IndexModel(ITeacherService repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            TeacherList = await _repo.GetAll();
            TeacherFList = Filter(TeacherList);

            UserID = HttpContext.Session.GetInt32("ID");
            if (UserID != null)
            {
                Teacher t = new Teacher { ID = (int)UserID };
                t = TeacherList.Find((t) => t.ID == UserID);
                if (t != null)
                {
                    IsUser = true;
                    TheTeacher = t;
                }
            }

            Role = HttpContext.Session.GetInt32("Role") ?? -1;
        }

        private IEnumerable<Teacher> Filter(IEnumerable<Teacher> tLst)
        {
            List<Predicate<Teacher>> predicates = new List<Predicate<Teacher>>();
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
            return FilterFunctions<Teacher>.Filter(tLst, predicates);
        }
    }
}
