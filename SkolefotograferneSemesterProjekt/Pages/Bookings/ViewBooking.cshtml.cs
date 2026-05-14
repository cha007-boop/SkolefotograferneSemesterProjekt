using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class ViewBookingModel : PageModel
    {
        #region Instance fields
        private IClassBookingService _classBookingService;
        #endregion
        #region Properties
        public ClassBooking TheClassBooking { get; set; }
        #endregion
        #region Constructor
        public ViewBookingModel(IClassBookingService classBookingService)
        {
            _classBookingService = classBookingService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                TheClassBooking = await _classBookingService.GetByID(id);
                return Page();
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Bookings/ListBookings");
            }
        }
        #endregion
    }
}
