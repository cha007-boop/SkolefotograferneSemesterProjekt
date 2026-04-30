using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class CreatePhotoEventsModel : PageModel
    {
        private IPhotoEventService _photoEventService;

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }

        public CreatePhotoEventsModel(IPhotoEventService photoEventService)
        {
            _photoEventService = photoEventService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost() /*possibly validation checker or exception check could be used here*/
        {
            try
            {
                await _photoEventService.Add(PhotoEvent);
            } 
            catch (SqlException ex)
            {
                Console.WriteLine( ex.Message);
                throw;
            }
            return RedirectToPage("/Index"); //return RedirectToPage("/Pages/PhotoEvents/Index"); 
        }
    }
}
