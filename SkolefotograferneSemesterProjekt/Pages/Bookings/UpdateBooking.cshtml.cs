using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class UpdateBookingModel : PageModel
    {
        IClassBookingService _classBookingService;
        IPhotoEventService _photoEventService;

        [BindProperty]
        public ClassBooking? TheClassBooking { get; set; }
        [BindProperty]
        public int? PhotoEventID { get; set; }
        public IEnumerable<SelectListItem> TimeSlots { get; set; }

        public UpdateBookingModel(IClassBookingService classBookingService, IPhotoEventService photoEventService)
        {
            _classBookingService = classBookingService;
            _photoEventService = photoEventService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            int? role = HttpContext.Session.GetInt32("Role");
            if (userID == null || role != 2)
            {
                return RedirectToPage("/Users/AccessDenied");
            }

            TheClassBooking = await _classBookingService.GetByID(id);
            if (TheClassBooking == null)
            {
                return RedirectToPage("/Bookings/ListBookings");
            }
            PhotoEventID = TheClassBooking.ThePhotoEvent.ID;

            await LoadMenus();
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            ModelState.CustomizedMessages("Feltet mangler");
            await LoadMenus();

            if (TheClassBooking == null)
            {
                return Page();
            }
            try
            {
                await _classBookingService.Update(TheClassBooking);
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
            if (PhotoEventID == null || PhotoEventID <= 0)
            {
                return;
            }

            PhotoEvent? photoEvent = await _photoEventService.GetByID((int)PhotoEventID);
            if (photoEvent == null)
            {
                return;
            }
            
            DateTime peCurrent = photoEvent.StartTime;
            DateTime peEnd = photoEvent.EndTime;
            List<SelectListItem> timeSlots = [];
            while (peCurrent.AddMinutes(20) <= peEnd)
            {
                ClassBooking temp = new ClassBooking() { ID = TheClassBooking.ID, StartTime = peCurrent };

                bool isAvailable = await _classBookingService.IsTimeAvailable(temp);
                if (isAvailable)
                {
                    timeSlots.Add(new SelectListItem
                    {
                        Value = peCurrent.ToString("yyyy-MM-dd THH:mm"),
                        Text = peCurrent.ToString("HH:mm")
                    });
                }
                peCurrent = peCurrent.AddMinutes(20);
            }
            TimeSlots = timeSlots;
        }
    }
}
