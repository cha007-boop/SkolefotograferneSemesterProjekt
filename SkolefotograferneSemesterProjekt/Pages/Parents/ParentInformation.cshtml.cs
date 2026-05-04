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
        private ISchoolService _schoolService;
        private ISchoolClassService _classService;

        [BindProperty]
        public int ID { get; set; }
        public Parent Parent { get; set; }

        [BindProperty]
        public List<Student> Students { get; set; }

        [BindProperty]
        public List<School> Schools { get; set; }

        public List<SchoolClass> Classes { get; set; }

        public ParentInformationModel(IParentServices parentService, IStudentService studentService, ISchoolService schoolService, ISchoolClassService classService)
        {
            _parentService = parentService;
            _studentService = studentService;
            _schoolService = schoolService;
            _classService = classService;
        }

        public async Task OnGet(int Id)
        {
            try
            {
                Parent = await _parentService.SearchParent(ID);
                Students = await _studentService.GetAllByParent(ID);

                Classes = await _classService.GetAll();
                Schools = await _schoolService.GetAll();

            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
        }
    }
}
