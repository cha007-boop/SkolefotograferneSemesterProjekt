using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class ListOfChildrenModel : PageModel
    {
        private IStudentService _studentService;
        private IParentServices _parentService;
        [BindProperty]
        public List<Student> Students { get; set; }

        public Parent TheParent { get; set; }

        public ListOfChildrenModel(IStudentService studentService, IParentServices parentService)
        {
            _studentService = studentService;
            _parentService = parentService;
        }
        public async Task OnGetAsync()
        {
            TheParent = await _parentService.SearchParent((int)HttpContext.Session.GetInt32("ID"));
            Students = await _studentService.GetAllByParent((int)HttpContext.Session.GetInt32("ID"));
        }
    }
}
