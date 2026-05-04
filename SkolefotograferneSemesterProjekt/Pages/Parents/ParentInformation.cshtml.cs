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

        public async Task OnGet(int Id)
        {
            try
            {
                Parent = await _parentService.SearchParent(ID);
                Students = await _studentService.GetAllByParent(ID);
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
        }
    }
}
