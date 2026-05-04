using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolAdmins
{
    public class CreateSchoolAdminModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;
        private ISchoolService _schoolService;

        [BindProperty]
        public SchoolAdmin NewSchoolAdmin { get; set; }

        [BindProperty]
        public string SchoolID { get; set; }

        [BindProperty]
        public string VerifyPassword { get; set; }

        public IEnumerable<SelectListItem> Schools { get; set; }
        public CreateSchoolAdminModel(ISchoolAdminService schoolAdminService, ISchoolService schoolService)
        {
            _schoolAdminService = schoolAdminService;
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

            NewSchoolAdmin.TheSchool = _schoolService.GetById(Convert.ToInt32(SchoolID)).Result;

            ModelState.Clear();
            TryValidateModel(NewSchoolAdmin);

            try
            {
                if (NewSchoolAdmin.Password != VerifyPassword)
                {
                    ModelState.AddModelError("VerifyPassword", "Passwords do not match");
                    await OnGet();
                    return Page();
                }

                await _schoolAdminService.Add(NewSchoolAdmin);
            }
            catch (TakenMailException Tex)
            {
                ModelState.AddModelError("NewSchoolAdmin.Email", Tex.Message);
                await OnGet();
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                await OnGet();
                return Page();
            }
            return RedirectToPage("GetAllSchoolAdmin");
        }
    }
}
