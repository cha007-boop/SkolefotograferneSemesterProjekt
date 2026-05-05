using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data.SqlTypes;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class CreatePhotoEventsModel : PageModel
    {
        private IPhotoEventService _photoEventService;

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        private string _queryStringPhotographerIDFinder = "Select from Photographer where PhotographerID = @PhotographerID";
        [BindProperty]
        public int VerifyPhotographerID { get; set; }
        [BindProperty]
        public int VerifySchoolAdminID { get; set; }

        public CreatePhotoEventsModel(IPhotoEventService photoEventService)
        {
            _photoEventService = photoEventService;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost() /*possibly validation checker or exception check could be used here*/
        {
            ModelState.Clear();
            TryValidateModel(PhotoEvent);
            try
            {
                //This is used to validate if the statements below is true or false - doesnt work atm
                //if (_queryStringPhotographerIDFinder != VerifyPhotographerID.ToString())
                //{
                //    ModelState.AddModelError("PhotoEvent.PhotographerID", "pls input an existing photographers id");
                //    return Page();
                //}
                //if (PhotoEvent.SchoolAdminID != VerifySchoolAdminID)
                //{
                //    ModelState.AddModelError("PhotoEvent.SchoolAdminID", "pls input an existing School admin's id");
                //    return Page();
                //}
                if (PhotoEvent.StartTime > PhotoEvent.EndTime) // this works
                {
                    ModelState.AddModelError("PhotoEvent", "The Date for StartTime needs to be before the Date of EndTime");
                    return Page();
                }
                else
                {
                    await _photoEventService.Add(PhotoEvent);
                }
            } 
            catch (SqlException ex)
            {
                ViewData["ErrorMessage"] = ex;
                ModelState.AddModelError("PhotoEvent", ex.Message);
                return Page();
            }
            catch (SqlTypeException tex)
            {
                ViewData["ErrorMessage"] = tex;
                ModelState.AddModelError("PhotoEvent.StartTime", tex.Message);
                return Page();
            }
            return RedirectToPage("/Index"); //return RedirectToPage("/Pages/PhotoEvents/Index"); 
        }
    }
}
