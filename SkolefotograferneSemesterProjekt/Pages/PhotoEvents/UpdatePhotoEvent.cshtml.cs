using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class UpdatePhotoEventModel : PageModel
    {
        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public int VerifyPhotographerID { get; set; }
        [BindProperty]
        public int VerifySchoolAdminID { get; set; }
        [BindProperty]
        private ISchoolAdminService _schoolAdminService { get; set; }
        [BindProperty]
        private IPhotographerService _photographerService { get; set; }
        private IPhotoEventService _photoEventService;
        [BindProperty]
        public IEnumerable<SelectListItem> Photographers { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> SchoolAdmins { get; set; }
        public UpdatePhotoEventModel(IPhotoEventService photoEventService, ISchoolAdminService schoolAdminService, IPhotographerService photographerService)
        {
            _schoolAdminService = schoolAdminService;
            _photographerService = photographerService;
            _photoEventService = photoEventService;
        }
        public async Task OnGet()
        {
            List<SchoolAdmin> schoolAdmins = await _schoolAdminService.GetAll();
            List<Photographer> photographers = await _photographerService.GetAll();
            SchoolAdmins = schoolAdmins.Select(s => new SelectListItem
            {
                Value = Convert.ToString(s.ID),
                Text = $"ID: {s.ID}, Name: {s.ContactPerson} - School: {s.TheSchool.Name}, PhoneNumber: {s.PhoneNumber}"
            });
            Photographers = photographers.Select(s => new SelectListItem
            {
                Value = Convert.ToString(s.ID),
                Text = $"ID: {s.ID}, Name: {s.FirstName} - CVR: {s.CVR}, PhoneNumber: {s.PhoneNumber}"
            });
        }
        public async Task OnPost()
        {

        }
    }
}
