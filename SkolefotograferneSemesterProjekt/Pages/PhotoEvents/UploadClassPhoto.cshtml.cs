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
        public IFormFile Photo { get; set; }

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
                return RedirectToPage("/Index");
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Photo != null)
            {
                Photo classPhoto = new Photo
                {
                    Filename = ProcessUploadedFile(),
                    ThePhotoEvent = this.ThePhotoEvent,
                   
                    TheSchoolClass = this.TheSchoolClass,
                    UploadedAt = DateTime.Now
                };


                await _photoService.Add(classPhoto);
            }
            return RedirectToPage("/PhotoEvents/PhotoEventDetails", new { id = ThePhotoEvent.ID });
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/ClassPhotos");
                uniqueFileName = TheSchoolClass.TheSchool.ID + "_" 
                    + TheSchoolClass.Grade + TheSchoolClass.Letter + "_" 
                    + Guid.NewGuid().ToString() + "_" + Photo.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
