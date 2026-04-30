using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data.SqlTypes;
using static System.Net.Mime.MediaTypeNames;

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
                ViewData["ErrorMessage"] = ex;
                throw;
            }
            catch (SqlTypeException tex)
            {
                ViewData["ErrorMessage"] = tex;
                throw;
            }
            return RedirectToPage("/Index"); //return RedirectToPage("/Pages/PhotoEvents/Index"); 
        }
    }
}
