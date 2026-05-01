using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class DeleteTeacherModel : PageModel
    {
        private ITeacherService _repo;

        [BindProperty]
        public Teacher? TeacherToDelete { get; set; }
        public string Message { get; set; }

        public DeleteTeacherModel(ITeacherService repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            TeacherToDelete = await _repo.GetByID(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            //TeacherToDelete = await _repo.GetByID(id);
            ModelState.Remove("TeacherToDelete.Password");
            if (!ModelState.IsValid)
            {
                
                return Page();
            }
            else if (TeacherToDelete == null)
            {
                Message = "Brugeren er null";
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
