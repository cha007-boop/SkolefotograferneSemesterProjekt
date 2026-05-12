using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

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
        public bool IsUser { get; set; }
        #endregion
        #region Constructor
        public ListBookingsModel(IClassBookingService classBookingService, ITeacherService teacherService, ISchoolAdminService schoolAdminService)
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
            if (!userID.HasValue || !role.HasValue)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

            Teacher? t = await _teacherService.GetByID(userID.Value);
            if (t == null)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            if(role == (int)UserRole.Teacher && t.ID > 0)
            {
                IsUser = userID == t.ID;
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
