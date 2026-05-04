using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;


namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class CreateTeacherModel : PageModel
    {
        private ITeacherService _repo;
        private ISchoolService _schoolService;

        [BindProperty]
        public Teacher NewTeacher { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Pass2 { get; set; }
        public IEnumerable<SelectListItem> Schools { get; set; }

        public CreateTeacherModel(ITeacherService repo, ISchoolService schoolService)
        {
            _repo = repo;
            _schoolService = schoolService;
        }
        public async Task OnGet()
        {
            List<School> schools = await _schoolService.GetAll();
            Schools = schools.Select(s => new SelectListItem
            {
                Value = Convert.ToString(s.ID),
                Text = $"{s.Name} - {s.Street} {s.ZipCode}"
            });
        }
        public async Task<IActionResult> OnPost()
        {
            await OnGet();
            ModelState.Remove("NewTeacher.Password");
            //ModelState.Remove("Password");
            ModelState.Remove("NewTeacher.TheSchool.ID");
            ModelState.Remove("NewTeacher.TheSchool.Name");
            ModelState.Remove("NewTeacher.TheSchool.Street");
            ModelState.Remove("NewTeacher.TheSchool.Country");
            ModelState.Remove("NewTeacher.TheSchool.ZipCode");    
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

            try
            {
                NewTeacher.Password = Password;
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
