using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class IndexModel : PageModel
    {
        public ITeacherService _repo;
        public List<Teacher> TeacherList { get; set; }
        public Teacher TheTeacher { get; set; }
        public int? UserID { get; set; }
        public bool IsUser { get; set; }

        public IndexModel(ITeacherService repo)
        {
            _repo = repo;
            
        }

        public async Task OnGet()
        {
            TeacherList = await _repo.GetAll();

            UserID = HttpContext.Session.GetInt32("ID");
            if (UserID != null)
            {
                Teacher t = new Teacher { ID = (int)UserID };
                t = TeacherList.Find((t) => t.ID == UserID)!;
                if (t != null)
                {
                    IsUser = true;
                    TheTeacher = t;
                }
            }
            
        }
    }
}
