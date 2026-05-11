using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class DeleteBookingModel : PageModel
    {
        IClassBookingService _classBookingService;

        [BindProperty]
        public ClassBooking? TheClassBooking { get; set; }
        public int? UserID { get; set; }
        public int? Role { get; set; } = -1;
        public DeleteBookingModel(IClassBookingService classBookingService)
        {
            _classBookingService = classBookingService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            UserID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");
            if (UserID == null || Role != 2)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

            TheClassBooking = await _classBookingService.GetByID(id);

            return Page();
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
