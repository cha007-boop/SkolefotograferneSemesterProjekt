using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class EditTeacherModel : PageModel
    {
        private ITeacherService _repo;
        private IUserService _userService;

        [BindProperty]
        public Teacher? TeacherToEdit { get; set; }

        public EditTeacherModel(ITeacherService repo, IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }
        public async Task OnGet(int id)
        {
            TeacherToEdit = await _repo.GetByID(id);
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("TeacherToEdit.Password");
            ModelState.CustomizedMessages("Feltet mangler");

            if (await _userService.IsEmailTaken(TeacherToEdit!))
            {
                ModelState.AddModelError("TeacherToEdit.Email", "Mailen er optaget");
                return Page();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _repo.Update(TeacherToEdit!);
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
