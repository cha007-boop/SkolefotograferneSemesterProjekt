using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class UploadPortraitModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IPhotoService _photoService;
        private IStudentService _studentService;
        private IPhotoEventService _photoEventService;

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }
        [BindProperty]
        public Student TheStudent { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PhotoEventId { get; set; }
        [BindProperty]
        public PhotoEvent ThePhotoEvent { get; set; }

        [BindProperty]
        public FileUpload FileUpload { get; set; }
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
            catch (UnauthorizedAccessException)
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

        
        public async Task<IActionResult> OnPostUploadMulti()
        {
            if (FileUpload.Files != null && FileUpload.Files.Count > 0)
            {
                try
                {
                    TheStudent = await _studentService.GetById(StudentId);
                    ThePhotoEvent = await _photoEventService.GetByID(PhotoEventId);

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Portraits");
                    foreach (var file in FileUpload.Files)
                    {
                        string uniqueFileName = $"{TheStudent.TheSchool.ID}_{TheStudent.TheSchoolClass.ID}_{TheStudent.ID}_" 
                            + Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Saving photo info to database
                        Photo photo = new Photo
                        {
                            Filename = uniqueFileName,
                            ThePhotoEvent = this.ThePhotoEvent,
                            Child = this.TheStudent,
                            TheSchoolClass = this.TheStudent.TheSchoolClass,
                            UploadedAt = DateTime.Now
                        };
                        await _photoService.Add(photo);
                    }
                    FileUpload.SuccessMessage = $"{FileUpload.Files.Count} Files uploaded successfully!";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FileUpload.Files", $"Upload error: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please select at least one file to upload.");
            }
            ViewData["SuccessMessage"] = FileUpload.SuccessMessage;
            return Page();
        }
    }

    public class FileUpload
    {
        [Required]
        [Display(Name = "File")]
        public List<IFormFile> Files { get; set; }
        public string SuccessMessage { get; set; } = string.Empty;
    }
}
