using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class DeleteTeacherModel : PageModel
    {
        private ITeacherService _repo;

        public Teacher? TeacherToDelete { get; set; }

        public DeleteTeacherModel(ITeacherService repo)
        {
            _repo = repo;
        }
        public async Task OnGet(int id)
        {
            TeacherToDelete = await _repo.GetByID(id);
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            TeacherToDelete = await _repo.GetByID(id);
            if (TeacherToDelete == null)
            {
                return Page();
            }
            try
            {
                await _repo.Delete(TeacherToDelete);
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Index");
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
    }
}
