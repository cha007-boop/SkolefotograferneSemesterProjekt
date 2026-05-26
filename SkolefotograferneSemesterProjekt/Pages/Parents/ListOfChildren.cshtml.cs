using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class ListOfChildrenModel : PageModel
    {
        private IStudentService _studentService;
        [BindProperty]
        public List<Student> Students { get; set; }

        public ListOfChildrenModel(IStudentService studentService)
        {
            _studentService = studentService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") != 0)
                {
                    throw new AccessViolationException();
                }
                Students = await _studentService.GetAllByParent((int)HttpContext.Session.GetInt32("ID"));
            }
            catch (AccessViolationException avx)
            {
                ViewData["ErrorMessage"] = avx.Message;
                return RedirectToPage("/Users/AccessDenied");
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return Page();
        }
    }
}
