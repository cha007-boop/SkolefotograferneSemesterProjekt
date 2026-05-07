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
        public ClassBooking NewBooking { get; set; } = new ClassBooking();
        [BindProperty]
        public SchoolClass SchoolClass { get; set; }
        [BindProperty]
        public PhotoEvent? ThePhotoEvent { get; set; } = new PhotoEvent();
        [BindProperty]
        public Teacher? TheTeacher { get; set; } = new Teacher();
        public IEnumerable<SelectListItem> Classes { get; set; }
        [BindProperty]
        public int? UserID { get; set; }
        public int? Role { get; set; }

        public CreateBookingModel(IClassBookingService classBookingService, ITeacherService teacherService, IPhotoEventService photoEventService, ISchoolClassService schoolClassService)
        {
            _repo = classBookingService;
            _teacherService = teacherService;
            _photoEventService = photoEventService;
            _schoolClassService = schoolClassService;
        }

        public async Task<IActionResult> OnGet()
        {
            UserID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");
            if(UserID == null || Role != 2)
            {
                return RedirectToPage("Index");
            }

            List<SchoolClass> classes = await _schoolClassService.GetAllByTeacher((int)UserID);
            Classes = classes.Select(c => new SelectListItem
            {
                Value = Convert.ToString(c.ID),
                Text = $"{c.Grade}.{c.Letter}"
            });

            return Page();
        }
        public async Task<IActionResult> OnPost(int id)
        {
            TheTeacher = await _teacherService.GetByID((int)UserID);
            if (TheTeacher == null)
            {
                return RedirectToPage("Index");
            }
            NewBooking.TheTeacher = TheTeacher;

            ThePhotoEvent = await _photoEventService.GetByID(id);
            if (ThePhotoEvent == null)
            {
                return RedirectToPage("Index");
            }
            NewBooking.ThePhotoEvent = ThePhotoEvent;

            SchoolClass = await _schoolClassService.GetByID(NewBooking.TheSchoolClass.ID);
            if (SchoolClass == null)
            {
                return RedirectToPage("Index");
            }
            NewBooking.TheSchoolClass = SchoolClass;

            try
            {
                await _repo.Book(NewBooking);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("ListBookings");
        }
    }
}
