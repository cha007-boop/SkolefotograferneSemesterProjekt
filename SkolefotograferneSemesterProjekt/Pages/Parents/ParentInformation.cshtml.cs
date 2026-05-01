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

        [BindProperty]
        public int ID { get; set; }
        public Parent parent { get; set; }

        [BindProperty]
        public List<Student> students { get; set; }

        [BindProperty]
        public List<School> schools { get; set; }

        public ParentInformationModel(IParentServices parentService, IStudentService studentService, ISchoolService schoolService)
        {
            _parentService = parentService;
            _studentService = studentService;
            _schoolService = schoolService;
        }

        public async Task OnGet()
        {
            try
            {
                parent = await _parentService.SearchParent(ID);
                students = await _studentService.GetAllByParent(ID);
                foreach (Student s in students)
                {
                    School school = await _schoolService.GetById(s.SchoolID);
                    schools.Add(school);
                }

            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
        }
    }
}
