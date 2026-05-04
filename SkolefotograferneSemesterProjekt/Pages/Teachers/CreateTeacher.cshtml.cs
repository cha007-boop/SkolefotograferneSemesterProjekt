using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using SkolefotograferneSemesterProjekt.Helpers;


namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class CreateTeacherModel : PageModel
    {
        private ITeacherService _repo;

        [BindProperty]
        public Teacher NewTeacher { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Pass2 { get; set; }
        public string Message { get; set; }

        public CreateTeacherModel(ITeacherService repo)
        {
            _repo = repo;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("NewTeacher.Password");
            ModelState.CustomizedMessages("Feltet mangler");

            if (Password != Pass2)
            {
                ModelState.AddModelError("Password", "Koderne er ikke ens");
            }
            if (Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Dit kodeord er for kort");
            }
            if (await _repo.IsEmailTaken(NewTeacher))
            {
                ModelState.AddModelError("NewTeacher.Email", "Mailen er optaget");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            NewTeacher.Password = Password;

            try
            {
                await _repo.Add(NewTeacher);
            }
            catch (PasswordTooShortException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            catch (TakenMailException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
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
