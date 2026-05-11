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
        private IUserService _userService;
        private ITeacherService _repo;
        private ISchoolService _schoolService;

        [BindProperty]
        public Teacher NewTeacher { get; set; } = new();
        [BindProperty]
        public string Password { get; set; } = "";
        [BindProperty]
        public string Pass2 { get; set; } = "";
        public int? Role { get; set; }
        public IEnumerable<SelectListItem> Schools { get; set; } = [];

        public CreateTeacherModel(ITeacherService repo, ISchoolService schoolService, IUserService userService)
        {
            _repo = repo;
            _schoolService = schoolService;
            _userService  = userService;
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
            ModelState.CustomizedMessages("Feltet mangler");

            if (!string.IsNullOrEmpty(Password))
            {
                if (Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Dit kodeord er for kort");
                }
                if (Password != Pass2)
                {
                    ModelState.AddModelError("Password", "Koderne er ikke ens");
                }
            }
            if(!string.IsNullOrEmpty(NewTeacher.Email))
            {
                if (await _userService.IsEmailTaken(NewTeacher))
                {
                    ModelState.AddModelError("NewTeacher.Email", "Mailen er optaget");
                }
            }
            if(NewTeacher.TheSchool == null || NewTeacher.TheSchool.ID <= 0)
            {
                ModelState.AddModelError("NewTeacher.TheSchool.ID", "Du skal vælge en skole");
            }
            else
            {
                ModelState.Remove("NewTeacher.TheSchool.Name");
                ModelState.Remove("NewTeacher.TheSchool.Street");
                ModelState.Remove("NewTeacher.TheSchool.Country");
                ModelState.Remove("NewTeacher.TheSchool.ZipCode");
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

            Role = HttpContext.Session.GetInt32("Role");
            if (Role  == (int)UserRole.SysAdmin)
            {
                return RedirectToPage("Index");
            }

            return RedirectToPage("/Users/Login");
        }
    }
}
