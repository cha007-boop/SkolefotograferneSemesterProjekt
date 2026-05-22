using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class UploadClassPhotoModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IPhotoService _photoService;
        private ISchoolClassService _schoolClassService;
        private IPhotoEventService _photoEventService;

        [BindProperty(SupportsGet = true)]
        public int SchoolClassId { get; set; }
        [BindProperty]
        public SchoolClass TheSchoolClass { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PhotoEventId { get; set; }
        [BindProperty]
        public PhotoEvent ThePhotoEvent { get; set; }

        [BindProperty]
        public List<IFormFile> Photos { get; set; }

        public UploadClassPhotoModel(IWebHostEnvironment webHostEnvironment, IPhotoService photoService, ISchoolClassService schoolClassService, IPhotoEventService photoEventService)
        {
            _webHostEnvironment = webHostEnvironment;
            _photoService = photoService;
            _schoolClassService = schoolClassService;
            _photoEventService = photoEventService;
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 1 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException();
                }

                TheSchoolClass = await _schoolClassService.GetByID(SchoolClassId);
                ThePhotoEvent = await _photoEventService.GetByID(PhotoEventId);

            }
            catch (UnauthorizedAccessException)
            {
                ViewData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToPage("/Users/AccessDenied");
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Users/AccessDenied");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Photos != null && Photos.Count > 0)
            {
                try
                {
                    TheSchoolClass = await _schoolClassService.GetByID(SchoolClassId);
                    ThePhotoEvent = await _photoEventService.GetByID(PhotoEventId);
                    IUploadIFormFile uploader = new UploadClassPhoto(_webHostEnvironment, TheSchoolClass);
                    foreach (var photo in Photos)
                    {
                        Photo classPhoto = new Photo
                        {
                            Filename = await uploader.UploadFile(photo),
                            ThePhotoEvent = this.ThePhotoEvent,
                            TheSchoolClass = this.TheSchoolClass,
                            UploadedAt = DateTime.Now
                        };
                        await _photoService.Add(classPhoto);
                    }
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while uploading the photo: {exc.Message}");
                    return Page();
                }

            }
            return RedirectToPage("/PhotoEvents/PhotoEventDetails", new { id = ThePhotoEvent.ID });
        }

    }
}
