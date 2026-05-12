using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class UploadPortraitModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IPhotoService _photoService;
        private IStudentService _studentService;
        private IPhotoEventService _photoEventService;

        [BindProperty(SupportsGet =true)]
        public int StudentId { get; set; }
        [BindProperty]
        public Student TheStudent { get; set; }
        [BindProperty(SupportsGet =true)]
        public int PhotoEventId { get; set; }
        [BindProperty]
        public PhotoEvent ThePhotoEvent { get; set; }

        [BindProperty]
        public IFormFile Portrait { get; set; }
        public UploadPortraitModel(IWebHostEnvironment webHostEnvironment, IPhotoService photoService, IStudentService studentService, IPhotoEventService photoEventService)
        {
            _webHostEnvironment = webHostEnvironment;
            _photoService = photoService;
            _studentService = studentService;
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

                TheStudent = await _studentService.GetById(StudentId);
                ThePhotoEvent = await _photoEventService.GetByID(PhotoEventId);

            }
            catch(UnauthorizedAccessException)
            {
                ViewData["ErrorMessage"] = "You do not have permission to access this page.";
                return RedirectToPage("/Users/AccessDenied");
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
            if (Portrait != null)
            {
                Photo portrait = new Photo
                {
                    Filename = ProcessUploadedFile(),
                    ThePhotoEvent = this.ThePhotoEvent,
                    Child = this.TheStudent,
                    TheSchoolClass = this.TheStudent.TheSchoolClass,
                    UploadedAt = DateTime.Now
                };


                await _photoService.Add(portrait);
            }
            return RedirectToPage("/PhotoEvents/PhotoEventDetails", new { id = ThePhotoEvent.ID });
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (Portrait != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Portraits");
                uniqueFileName =  TheStudent.TheSchool.ID + "_" 
                    + TheStudent.TheSchoolClass.Grade + TheStudent.TheSchoolClass.Letter + "_" 
                    + TheStudent.ID + "_" + Guid.NewGuid().ToString() + "_" + Portrait.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Portrait.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
