using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Schools
{
    public class CreateSchoolModel : PageModel
    {
        private ISchoolService _schoolService;

        [BindProperty] 
        public School NewSchool { get; set; }
        public CreateSchoolModel(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _schoolService.Add(NewSchool);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("GetAllSchool");
        }
    }
}
