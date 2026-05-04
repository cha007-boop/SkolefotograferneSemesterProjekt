using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers.Filter;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Pages.Students
{
    public class ListStudentsModel : PageModel
    {
        #region Instance Fields
        private IStudentService _studentService;
        #endregion
        #region Properties
        [BindProperty]
        public IEnumerable<Student> Students { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }
        #endregion
        #region Constructor
        public ListStudentsModel(IStudentService studentService)
        {
            _studentService = studentService;
        }
        #endregion
        #region Methods
        public async Task OnGet()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") == 0)
                {
                    Students = await _studentService.GetAllByParent((int)HttpContext.Session.GetInt32("ID"));
                }
                if (HttpContext.Session.GetInt32("Role") == 1)
                {
                    throw new NotImplementedException();
                }
                if (HttpContext.Session.GetInt32("Role") == 4)
                {
                    Students = FilterStudents(await _studentService.GetAll());
                }
            }
            catch(Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
            }
        }

        private IEnumerable<Student> FilterStudents(IEnumerable<Student> students)
        {
            List<Predicate<Student>> predicates = new List<Predicate<Student>>();
            if (!string.IsNullOrWhiteSpace(FilterCriteria))
            {
                switch (FilterBy)
                {
                    case "FirstName":
                        predicates.Add(b => !string.IsNullOrEmpty(b.FirstName) && b.FirstName.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "SurName":
                        predicates.Add(b => !string.IsNullOrEmpty(b.Surname) && b.Surname.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "TheSchool":
                        predicates.Add(b => !string.IsNullOrEmpty(b.TheSchool.Name) && b.TheSchool.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "TheParent":
                        predicates.Add(b => !string.IsNullOrEmpty(b.TheParent.ID.ToString()) && b.TheParent.ID.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    default:
                        break;
                }
            }
            return FilterFunctions<Student>.Filter(students, predicates);
        }
        #endregion
    }
}
