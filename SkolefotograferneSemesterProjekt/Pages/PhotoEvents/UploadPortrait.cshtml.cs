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
        public Student TheStudent { get; set; }
        [BindProperty(SupportsGet =true)]
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

        public async Task<IActionResult> OnGet(int studentid, int photoeventid)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 1 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException();
                }

                TheStudent = await _studentService.GetById(studentid);
                ThePhotoEvent = await _photoEventService.GetByID(photoeventid);

            }
            catch(UnauthorizedAccessException)
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
            ThePhotoEvent = await _photoEventService.GetByID(ThePhotoEvent.ID);
            TheStudent = await _studentService.GetById(TheStudent.ID);

            if (Portrait != null)
            {
                Photo portrait = new Photo 
                { 
                    Filename = ProcessUploadedFile(), 
                    ThePhotoEvent = this.ThePhotoEvent, 
                    Child = this.TheStudent, 
                    TheSchoolClass = this.TheStudent.TheSchoolClass 
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
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Portrait.FileName;
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
