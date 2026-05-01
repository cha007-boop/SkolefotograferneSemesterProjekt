using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.TeacherCRUD
{
    public class GetAllTeachersModel : PageModel
    {
        public ITeacherService _repo;
        public List<Teacher> TeacherList { get; set; }

        public GetAllTeachersModel(ITeacherService repo)
        {
            _repo = repo;
        }

        public async Task OnGet()
        {
            TeacherList = await _repo.GetAll();
        }
    }
}
