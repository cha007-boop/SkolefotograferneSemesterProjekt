using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class PhotoEventDetailsModel : PageModel
    {
        private IPhotoEventService _photoEventService;
        private ISchoolClassService _schoolClassService;

        public PhotoEvent ThePhotoEvent { get; set; }

        public PhotoEventDetailsModel(IPhotoEventService photoEventService, ISchoolClassService schoolClassService)
        {
            _photoEventService = photoEventService;
            _schoolClassService = schoolClassService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            ThePhotoEvent = await _photoEventService.GetByID(id);
            if (ThePhotoEvent == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
