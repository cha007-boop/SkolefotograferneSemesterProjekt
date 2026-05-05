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
        [BindProperty]
        public List<ClassBooking> Bookings { get; set; }
        public Teacher ThisTeacher { get; set; }
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
            try
            {
                if (HttpContext.Session.GetInt32("Role") == 2)
                {
                    foreach(ClassBooking booking in await _classBookingService.GetBookingsByTeacher(await _teacherService.GetByID((int)HttpContext.Session.GetInt32("ID"))))
                    {
                        if(booking.StartTime > DateTime.Now)
                        {
                            Bookings.Add(booking);
                        }
                    }
                }
                //if (HttpContext.Session.GetInt32("Role") == 3)
                //{
                //    throw new NotImplementedException();
                //}
                //if (HttpContext.Session.GetInt32("Role") == 4)
                //{
                //    Bookings = await _classBookingService.GetAll();
                //}
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
