using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class CreateBookingModel : PageModel
    {
        private IClassBookingService _repo;
        private ITeacherService _teacherService;
        private IPhotoEventService _photoEventService;
        private ISchoolClassService _schoolClassService;

        [BindProperty]
        public ClassBooking NewBooking { get; set; }
        public PhotoEvent? ThePhotoEvent { get; set; }
        public Teacher? TheTeacher { get; set; }
        public IEnumerable<SelectListItem> Classes { get; set; }
        public int? UserID { get; set; }
        public int? Role { get; set; }

        public CreateBookingModel(IClassBookingService classBookingService, ITeacherService teacherService, IPhotoEventService photoEventService, ISchoolClassService schoolClassService)
        {
            _repo = classBookingService;
            _teacherService = teacherService;
            _photoEventService = photoEventService;
            _schoolClassService = schoolClassService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            ThePhotoEvent = await _photoEventService.GetByID(id);

            UserID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");

            if(UserID == null || Role != 2)
            {
                return RedirectToPage("Index");
            }
            TheTeacher = await _teacherService.GetByID((int)UserID);
            
            List<SchoolClass> classes = await _schoolClassService.GetAllByTeacher(TheTeacher.ID);
            Classes = classes.Select(c => new SelectListItem
            {
                Value = Convert.ToString(c.ID),
                Text = $"{c.Grade}.{c.Letter}"
            });

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                //await _repo.Book(d);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
