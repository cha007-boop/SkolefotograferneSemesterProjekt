using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class EditTeacherModel : PageModel
    {
        private ITeacherService _repo;

        [BindProperty]
        public Teacher TeacherToEdit { get; set; }
        [BindProperty]
        public bool IsMailTaken { get; set; }
        public string Message { get; set; }

        public EditTeacherModel(ITeacherService repo)
        {
            _repo = repo;
        }

        public async Task OnGet(int id, List<string> takenEmails)
        {
            TeacherToEdit = await _repo.GetByID(id);
        }

        public async Task<IActionResult> OnPost()
        {
            IsMailTaken = await _repo.IsEmailTaken(TeacherToEdit);

            ModelState.Remove("TeacherToEdit.Password");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (IsMailTaken)
            {
                Message = "Mailen er optaget, prøv en anden mail...";
                return Page();
            }
            try
            {
                await _repo.Update(TeacherToEdit);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
