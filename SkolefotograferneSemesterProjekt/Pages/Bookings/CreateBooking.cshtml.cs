using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class CreateBookingModel : PageModel
    {
        private IClassBookingService _repo;
        private ITeacherService _teacherService;
        private IPhotoEventService _photoEventService;

        [BindProperty]
        public ClassBooking NewBooking { get; set; }
        [BindProperty]
        public PhotoEvent? ThePhotoEvent { get; set; }

        public CreateBookingModel(IClassBookingService classBookingService, ITeacherService teacherService, IPhotoEventService photoEventService)
        {
            _repo = classBookingService;
            _teacherService = teacherService;
            _photoEventService = photoEventService;
        }

        public async Task OnGet(int id )
        {
            ThePhotoEvent = await _photoEventService.GetByID(id);
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                //await _repo.Book(d);
            }
            catch (PasswordTooShortException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            catch (TakenMailException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
