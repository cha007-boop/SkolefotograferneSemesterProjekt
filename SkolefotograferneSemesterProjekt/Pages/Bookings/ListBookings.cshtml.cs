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
        #endregion
        #region Properties
        public List<ClassBooking> Bookings { get; set; }
        #endregion
        #region Constructor
        public ListBookingsModel(IClassBookingService classBookingService)
        {
            _classBookingService = classBookingService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") == 2)
                {
                    //Bookings = await _classBookingService.GetByTeacherID(HttpContext.Session.GetInt32("ID"));
                }
                if (HttpContext.Session.GetInt32("Role") == 3)
                {
                    throw new NotImplementedException();
                }
                if (HttpContext.Session.GetInt32("Role") == 4)
                {
                    //Bookings = await _classBookingService.GetAll();
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
