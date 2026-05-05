using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Students
{
    public class UpdateStudentModel : PageModel
    {
        #region Instance fields
        private IStudentService _studentService;
        #endregion
        #region Properties
        [BindProperty]
        public Student NewStudent { get; set; }
        #endregion
        #region Constructor
        public UpdateStudentModel(IStudentService studentService)
        {
            _studentService = studentService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") == null)
                {
                    throw new Exception();
                }
                NewStudent = await _studentService.GetById(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public void OnPost()
        {

        }
        #endregion
    }
}
