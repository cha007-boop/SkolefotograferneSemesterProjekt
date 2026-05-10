using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class ListBookingsModel : PageModel
    {
        #region Instance fields
        private IClassBookingService _classBookingService;
        private ITeacherService _teacherService;
        #endregion
        #region Properties
        public List<ClassBooking> Bookings { get; set; } = [];
        #endregion
        #region Constructor
        public ListBookingsModel(IClassBookingService classBookingService, ITeacherService teacherService)
        {
            _classBookingService = classBookingService;
            _teacherService = teacherService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGetAsync()
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            int? role = HttpContext.Session.GetInt32("Role");
            if (userID == null || role == null)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            if (role != 2)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

            Teacher? t = await _teacherService.GetByID((int)userID);
            if (t == null)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            try
            {
                foreach (ClassBooking booking in await _classBookingService.GetBookingsByTeacher(t))
                {
                    if (booking.StartTime.Date >= DateTime.Today)
                    {
                        Bookings.Add(booking);
                    }
                }
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }
        #endregion
    }
}
