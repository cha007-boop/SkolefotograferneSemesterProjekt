using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

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
        public bool PassCheck { get; set; } = true;
        public string MsgPassword { get; set; }

        public CreateTeacherModel(ITeacherService repo)
        {
            _repo = repo;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(Pass2) || Password != Pass2)
            {

                MsgPassword = "Koderne er ikke ens eller tomme";

                return Page();
            }
            else
            {
                NewTeacher.Password = Password;
                try
                {
                    await _repo.Add(NewTeacher);
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
}
