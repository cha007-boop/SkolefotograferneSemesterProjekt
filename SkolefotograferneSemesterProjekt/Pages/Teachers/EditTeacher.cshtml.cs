using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class EditTeacherModel : PageModel
    {
        private ITeacherService _teacherService;
        private IUserService _userService;

        [BindProperty]
        public Teacher? TeacherToEdit { get; set; }

        public EditTeacherModel(ITeacherService teacherService, IUserService userService)
        {
            _teacherService = teacherService;
            _userService = userService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            TeacherToEdit = await _teacherService.GetByID(id);
            if(TeacherToEdit == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            ModelState.Remove("TeacherToEdit.Password");
            ModelState.Remove("TeacherToEdit.TheSchool.Name");
            ModelState.Remove("TeacherToEdit.TheSchool.Street");
            ModelState.Remove("TeacherToEdit.TheSchool.Country");
            ModelState.Remove("TeacherToEdit.TheSchool.ZipCode");
            ModelState.CustomizedMessages("Feltet mangler");

            if (!string.IsNullOrEmpty(TeacherToEdit.Email))
            {
                if (await _userService.IsEmailTaken(TeacherToEdit!))
                {
                    ModelState.AddModelError("TeacherToEdit.Email", "Mailen er optaget");
                    return Page();
                }
            
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _teacherService.Update(TeacherToEdit!);
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
