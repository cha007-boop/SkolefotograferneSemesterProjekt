using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Teachers
{
    public class DeleteTeacherModel : PageModel
    {
        private ITeacherService _teacherService;

        public Teacher? TeacherToDelete { get; set; }

        public DeleteTeacherModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        public async Task OnGet(int id)
        {
            TeacherToDelete = await _teacherService.GetByID(id);
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            TeacherToDelete = await _teacherService.GetByID(id);
            if (TeacherToDelete == null)
            {
                return Page();
            }
            try
            {
                await _teacherService.Delete(TeacherToDelete);
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
