using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolAdmins
{
    public class GetAllSchoolAdminModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;

        public List<SchoolAdmin> SchoolAdmins { get; set; }

        public GetAllSchoolAdminModel(ISchoolAdminService schoolAdminService)
        {
            _schoolAdminService = schoolAdminService;
        }

        public async Task<ActionResult> OnGet()
        {
            try
            {
                SchoolAdmins = await _schoolAdminService.GetAll();
            }
            catch
            {

            }
            return Page();
        }
    }
}
