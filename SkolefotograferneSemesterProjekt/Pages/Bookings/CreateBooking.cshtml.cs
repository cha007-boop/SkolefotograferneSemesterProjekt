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
        private IClassBookingService _classBookingService;
        private ITeacherService _teacherService;
        private IPhotoEventService _photoEventService;
        private ISchoolClassService _schoolClassService;

        [BindProperty]
        public ClassBooking NewBooking { get; set; } = new ClassBooking();
        [BindProperty]
        public int PhotoEventID { get; set; }
        public PhotoEvent? ThePhotoEvent { get; set; }
        public Teacher? TheTeacher { get; set; }
        public IEnumerable<SelectListItem> Classes { get; set; } = [];
        public IEnumerable<SelectListItem> TimeSlots { get; set; } = [];

        public CreateBookingModel(IClassBookingService classBookingService, ITeacherService teacherService, IPhotoEventService photoEventService, ISchoolClassService schoolClassService)
        {
            _classBookingService = classBookingService;
            _teacherService = teacherService;
            _photoEventService = photoEventService;
            _schoolClassService = schoolClassService;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id.HasValue)
            {
                int? userID = HttpContext.Session.GetInt32("ID");
                int? role = HttpContext.Session.GetInt32("Role");
                if (!userID.HasValue || role != (int)UserRole.Teacher)
                {
                    return RedirectToPage("/Users/AccessDenied");
                }

                PhotoEventID = id.Value;
                ThePhotoEvent = await _photoEventService.GetByID(id.Value);
                if (ThePhotoEvent == null)
                {
                    return RedirectToPage("Index");
                }

                TheTeacher = await _teacherService.GetByID(userID.Value);
                int? tSchoolID = TheTeacher?.TheSchool?.ID;
                int? saSchoolID = ThePhotoEvent.TheSchoolAdmin?.TheSchool.ID;
                if (saSchoolID.HasValue && tSchoolID.HasValue)
                {
                    bool sameSchool = tSchoolID == saSchoolID;
                    if (!sameSchool)
                    {
                        return RedirectToPage("/Users/AccessDenied");
                    }
                }

                await LoadMenus();
                return Page();
            }
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPost()
        {
            ModelState.CustomizedMessages("Feltet Mangler");

            int? userID = HttpContext.Session.GetInt32("ID");
            int? role = HttpContext.Session.GetInt32("Role");
            if (!userID.HasValue || role != (int)UserRole.Teacher)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

            TheTeacher = await _teacherService.GetByID(userID.Value);
            if (TheTeacher == null)
            {
                return RedirectToPage("Index");
            }
            NewBooking.TheTeacher = TheTeacher;

            if(PhotoEventID <= 0)
            {
                return RedirectToPage("Index");
            }

            ThePhotoEvent = await _photoEventService.GetByID(PhotoEventID);
            if (ThePhotoEvent == null)
            {
                return RedirectToPage("Index");
            }
            NewBooking.ThePhotoEvent = ThePhotoEvent;


            await LoadMenus();
            SchoolClass? schoolClass = await _schoolClassService.GetByID(NewBooking.TheSchoolClass.ID);
            if (schoolClass == null)
            {
                return Page();
            }
            NewBooking.TheSchoolClass = schoolClass;

            try
            {
                await _classBookingService.Book(NewBooking);
            }
            catch(BookingTimeNotAvailableException ex)
            {
                ModelState.AddModelError("NewBooking.StartTime", ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("ListBookings");
        }
        private async Task LoadMenus()
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if (!userID.HasValue || ThePhotoEvent == null)
            {
                return;
            }
            List<SchoolClass> classes = await _schoolClassService.GetAllByTeacher(userID.Value);
            Classes = classes.Select(c => new SelectListItem
            {
                Value = Convert.ToString(c.ID),
                Text = $"{c.Grade}.{c.Letter}"
            });

            DateTime peCurrent = ThePhotoEvent.StartTime;
            DateTime peEnd = ThePhotoEvent.EndTime;
            List<SelectListItem> timeSlots = [];
            while (peCurrent.AddMinutes(20) <= peEnd)
            {
                ClassBooking temp = new ClassBooking() { StartTime = peCurrent };

                bool isAvailable = await _classBookingService.IsTimeAvailable(temp);
                if (isAvailable)
                {
                    timeSlots.Add(new SelectListItem
                    {
                        Value = peCurrent.ToString("dd/MM/yyyy HH:mm"),
                        Text = peCurrent.ToString("HH:mm")
                    });
                }
                peCurrent = peCurrent.AddMinutes(20);
            }

            TimeSlots = timeSlots;
        }
    }
}
