using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Schools
{
    public class GetAllSchoolModel : PageModel
    {
        private ISchoolService _schoolService;

        public List<School> Schools { get; set; }
        public GetAllSchoolModel(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Schools = await _schoolService.GetAll();
            }
            catch
            {

            }
            return Page();
        }
    }
}
