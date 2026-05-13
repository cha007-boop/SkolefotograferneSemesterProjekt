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
        public DeleteBookingModel(IClassBookingService classBookingService)
        {
            _classBookingService = classBookingService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            int? role = HttpContext.Session.GetInt32("Role");
            if (!userID.HasValue || role != (int)UserRole.Teacher)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

            TheClassBooking = await _classBookingService.GetByID(id);
            if (TheClassBooking == null)
            {
                return RedirectToPage("/Users/AccesDenied");
            }

            int? tID = TheClassBooking?.TheTeacher?.ID;
            if(!tID.HasValue || tID != userID)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

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
