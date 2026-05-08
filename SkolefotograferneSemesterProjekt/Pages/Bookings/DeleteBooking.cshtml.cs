using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class DeleteBookingModel : PageModel
    {
        IClassBookingService _classBookingService;

        [BindProperty]
        public ClassBooking TheClassBooking { get; set; }

        public DeleteBookingModel(IClassBookingService classBookingService)
        {
            _classBookingService = classBookingService;
        }

        public async Task OnGet(int id)
        {
            TheClassBooking = await _classBookingService.GetByID(id);
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _classBookingService.Delete(TheClassBooking);
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
