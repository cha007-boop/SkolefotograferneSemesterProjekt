using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.TeacherCRUD
{
    public class EditTeacherModel : PageModel
    {
        private ITeacherService _repo;

        public Teacher TeacherToEdit { get; set; }

        public EditTeacherModel(ITeacherService repo)
        {
            _repo = repo;
        }

        public void OnGet()
        {
        }
    }
}
