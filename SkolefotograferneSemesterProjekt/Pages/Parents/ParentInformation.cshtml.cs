using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class ParentInformationModel : PageModel
    {
        private IParentServices _parentService;
        private IStudentService _studentService;

        [BindProperty]
        public int ID { get; set; }
        public Parent Parent { get; set; }

        [BindProperty]
        public List<Student> Students { get; set; }

        public ParentInformationModel(IParentServices parentService, IStudentService studentService)
        {
            _parentService = parentService;
            _studentService = studentService;
        }

        public async Task<IActionResult> OnGet(int Id)
        {
            try
            {
                if ( HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this page.");
                }
                Parent = await _parentService.SearchParent(Id);
                Students = await _studentService.GetAllByParent(Id);
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return RedirectToPage("/Users/AccessDenied");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return Page();
        }
    }
}
