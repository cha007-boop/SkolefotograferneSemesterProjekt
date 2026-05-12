using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class ListOfChildrenModel : PageModel
    {
        private IStudentService _studentService;
        //private IParentServices _parentService;
        [BindProperty]
        public List<Student> Students { get; set; }

        //public Parent TheParent { get; set; }

        public ListOfChildrenModel(IStudentService studentService/*, IParentServices parentService*/)
        {
            _studentService = studentService;
            //_parentService = parentService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") != 0)
                {
                    throw new AccessViolationException();
                }
                //TheParent = await _parentService.SearchParent((int)HttpContext.Session.GetInt32("ID"));
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
