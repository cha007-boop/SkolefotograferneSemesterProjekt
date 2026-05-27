using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Globalization;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class CreateBookingModel : PageModel
    {
        private IClassBookingService _classBookingService;
        private ITeacherService _teacherService;
        private IPhotoEventService _photoEventService;
        private ISchoolClassService _schoolClassService;

        private const int BookingDurationMinutes = 20;
        private const int SchoolDayStartHour = 8;
        private const int SchoolDayEndHour = 15;

        [BindProperty]
        public ClassBooking NewBooking { get; set; } = new ClassBooking();
        [BindProperty]
        public PhotoEvent? ThePhotoEvent { get; set; }
        //public int PhotoEventID { get; set; }
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
                if (HttpContext.Session.GetInt32("Role") != (int)UserRole.Teacher)
                {
                    return RedirectToPage("/Users/AccessDenied");
                }

                ThePhotoEvent = await _photoEventService.GetByID(id.Value);
                if (ThePhotoEvent == null)
                {
                    return RedirectToPage("Index");
                }

                TheTeacher = await _teacherService.GetByID((int)HttpContext.Session.GetInt32("ID"));

                if (TheTeacher?.TheSchool.ID != ThePhotoEvent.TheSchoolAdmin?.TheSchool.ID)
                {
                    return RedirectToPage("/Users/AccessDenied");
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
            ThePhotoEvent = await _photoEventService.GetByID(ThePhotoEvent.ID);
            if (TheTeacher == null)
            {
                return RedirectToPage("ListBookings");
            }
            NewBooking.TheTeacher = TheTeacher;

            if (ThePhotoEvent.ID <= 0)
            {
                return RedirectToPage("ListBookings");
            }

            if (ThePhotoEvent == null)
            {
                return RedirectToPage("ListBookings");
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
            catch (BookingTimeNotAvailableException ex)
            {
                ModelState.AddModelError("NewBooking.StartTime", ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("/PhotoEvents/ReadPhotoEvents");
        }
        private async Task LoadMenus()
        {
            if (ThePhotoEvent == null)
            {
                return;
            }
            List<SchoolClass> classes = await _schoolClassService.GetAllByTeacher((int)HttpContext.Session.GetInt32("ID"));
            Classes = classes.Select(c => new SelectListItem
            {
                Value = Convert.ToString(c.ID),
                Text = $"{c.Grade}.{c.Letter}"
            });

            DateTime peCurrent = ThePhotoEvent.StartTime;
            if (peCurrent.DayOfWeek == DayOfWeek.Sunday)
            {
                peCurrent = peCurrent.AddDays(1).Date.AddHours(SchoolDayStartHour);
            }
            // Make sure time slots minutes start on mulitples of BookingDurationMinutes (e.g., 0, 20, 40)
            if (peCurrent.Minute % BookingDurationMinutes != 0)
                peCurrent = peCurrent.AddMinutes(BookingDurationMinutes - (peCurrent.Minute % BookingDurationMinutes));

            DateTime peEnd = ThePhotoEvent.EndTime;
            List<SelectListItem> timeSlots = [];
            CultureInfo culture = new CultureInfo("da-DK");
            timeSlots.Add(new SelectListItem
            {
                Value = "",
                Text = $"------ {culture.DateTimeFormat.GetDayName(peCurrent.DayOfWeek)} ------",
                Disabled = true
            });
            while (peCurrent.AddMinutes(BookingDurationMinutes) <= peEnd)
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
                peCurrent = peCurrent.AddMinutes(BookingDurationMinutes);
                if (peCurrent.Hour >= SchoolDayEndHour)
                {
                    peCurrent = peCurrent.Date.AddDays(1).AddHours(SchoolDayStartHour);
                    if (peCurrent.DayOfWeek == DayOfWeek.Sunday)
                    {
                        peCurrent = peCurrent.AddDays(1);
                    }
                    if (peCurrent <= peEnd)
                    {
                        timeSlots.Add(new SelectListItem
                        {
                            Value = "",
                            Text = $"------ {culture.DateTimeFormat.GetDayName(peCurrent.DayOfWeek)} ------",
                            Disabled = true
                        });
                    }
                }

            }
            TimeSlots = timeSlots;
        }
    }
}
