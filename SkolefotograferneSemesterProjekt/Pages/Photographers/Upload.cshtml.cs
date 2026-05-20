using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Pages.Photographers
{
    public class UploadModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IPhotoService _photoService;



        [BindProperty]
        public FileUpload fileUpload { get; set; } 
        public UploadModel(IWebHostEnvironment webHostEnvironment, IPhotoService photoService)
        {
            _webHostEnvironment = webHostEnvironment;
            _photoService = photoService;
        }


        public IActionResult OnGet()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 1 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException();
                }
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

    //    public IActionResult OnPostUpload(FileUpload fileUpload)
    //    {
    //        try
    //        {
    //            if (HttpContext.Session.GetInt32("Role") != 1 && HttpContext.Session.GetInt32("Role") != 4)
    //            {
    //                throw new UnauthorizedAccessException();
    //            }
    //            if (fileUpload.Files != null && fileUpload.Files.Count > 0)
    //            {
    //                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
    //                Directory.CreateDirectory(uploadsFolder);
                    


    //                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fileUpload.FileName);
    //                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
    //                using (var fileStream = new FileStream(filePath, FileMode.Create))
    //                {
    //                    UploadedFile.CopyTo(fileStream);
    //                }
    //                Photo newPhoto = new Photo
    //                {
    //                    Filename = uniqueFileName,
    //                    UploadedAt = DateTime.Now
    //                };
    //                _photoService.Add(newPhoto);
    //            }
    //        }
    //        catch (UnauthorizedAccessException)
    //        {
    //            ViewData["ErrorMessage"] = "You do not have permission to access this page.";
    //            return RedirectToPage("/Users/AccessDenied");
    //        }
    //        catch (Exception exc)
    //        {
    //            ViewData["ErrorMessage"] = exc.Message;
    //            return RedirectToPage("/Index");
    //        }
    //        return RedirectToPage("/Photographers/Index");
    //    }
    }
    public class FileUpload
    {
        [Required]
        [Display(Name = "File")]
        public List<IFormFile> Files { get; set; }
        public string SuccessMessage { get; set; } = string.Empty;
    }
}
