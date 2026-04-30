using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class CreatePhotoEventsModel : PageModel
    {
        private IPhotoEventService _photoEventService;
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }

        public CreatePhotoEventsModel(IPhotoEventService photoEventService, IWebHostEnvironment webHostEnvironment)
        {
            _photoEventService = photoEventService;
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _photoEventService.Add(PhotoEvent);
            }
            catch   /*possibly validation checker or exception check needed here*/
            {
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
