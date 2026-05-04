using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers.Filter;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Students
{
    public class ListStudentsModel : PageModel
    {
        #region Instance Fields
        private IStudentService _studentService;
        #endregion
        #region Properties
        [BindProperty]
        public List<Student> Students { get; set; }
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
            if(HttpContext.Session.GetInt32("UserRole") == 0)
            {
                Students = await _studentService.GetAllByParent((int)HttpContext.Session.GetInt32("ID"));
            }
            if(HttpContext.Session.GetInt32("UserRole") == 1)
            {
                throw new NotImplementedException();
            }
            if (HttpContext.Session.GetInt32("UserRole") == 4)
            {
                Students = await _studentService.GetAll();
            }
        }
        #endregion
    }
}
