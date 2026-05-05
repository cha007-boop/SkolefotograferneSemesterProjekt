using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing.Constraints;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class StudentPicturesModel : PageModel
    {
        private IStudentService _studentService;
        private IPhotoService _photoservice;
        [BindProperty]
        public Parent TheParent { get; set; }
        [BindProperty]
        public Student TheStudent { get; set; }
        [BindProperty]
        public List<Photo> Photos { get; set; }

        public StudentPicturesModel(IStudentService studentService, IPhotoService photoService)
        {
            _studentService = studentService;
            _photoservice = photoService;
            Photos = new List<Photo>();
        }

        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 0 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this page.");
                }
                TheStudent = await _studentService.GetById(id);
                TheParent = TheStudent.TheParent;
                foreach (Photo ph in await _photoservice.GetPortraitsByStudentId(id))
                {
                    if (ph != null)
                        Photos.Add(ph);
                }
                if (Photos.Count == 0)
                {
                    Photo photo = new Photo
                    {
                        Filename = "NoImage.png",
                        ThePhotoEvent = new PhotoEvent(),
                        TheSchoolClass = new SchoolClass(),
                        Child = TheStudent
                    };
                }
            }
            catch (UnauthorizedAccessException uex)
            {
                ViewData["Errormessage"] = uex.Message;
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return RedirectToPage("/Parents/ParentInformation");
            }
            return Page();
        }
    }
}
