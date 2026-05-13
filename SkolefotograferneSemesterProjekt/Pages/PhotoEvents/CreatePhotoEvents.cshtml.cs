using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class CreatePhotoEventsModel : PageModel
    {
        private IPhotoEventService _photoEventService;
        private ISchoolAdminService _schoolAdminService;
        private IPhotographerService _photographerService;

        private string _queryStringPhotographerIDFinder = "Select from Photographer where PhotographerID = @PhotographerID";

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public int VerifyPhotographerID { get; set; }
        [BindProperty]
        public int VerifySchoolAdminID { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> Photographers { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> SchoolAdmins { get; set; }

        public CreatePhotoEventsModel(IPhotoEventService photoEventService, IPhotographerService photographerService, ISchoolAdminService schoolAdminService)
        {
            _photoEventService = photoEventService;
            _schoolAdminService = schoolAdminService;
            _photographerService = photographerService;
        }
        public async Task OnGet(int id)
        {
            List<SchoolAdmin> schoolAdmins = await _schoolAdminService.GetAll();
            List<Photographer> photographers = await _photographerService.GetAll();
            SchoolAdmins = schoolAdmins.Select(s => new SelectListItem
            {
                Value = Convert.ToString(s.ID),
                Text = $"ID: {s.ID}, Name: {s.ContactPerson} - School: {s.TheSchool.Name}, PhoneNumber: {s.PhoneNumber}"
            });
            Photographers = photographers.Select(s => new SelectListItem
            {
                Value = Convert.ToString(s.ID),
                Text = $"ID: {s.ID}, Name: {s.FirstName} - CVR: {s.CVR}, PhoneNumber: {s.PhoneNumber}"
            });
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
                if(HttpContext.Session.GetInt32("Role") == 3)
                {
                    var id = HttpContext.Session.GetInt32("ID");
                    if (!id.HasValue)
                    {
                        ModelState.AddModelError("", "Session ID missing.");
                        return Page();
                    }
                    PhotoEvent.TheSchoolAdmin = await _schoolAdminService.GetById(id.Value);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
                if (PhotoEvent.StartTime > PhotoEvent.EndTime) // this works
                {
                    ModelState.AddModelError("PhotoEvent", "The Date for StartTime needs to be before the Date of EndTime");
                    return Page();
                }
                if (PhotoEvent.StartTime >= DateTime.Now)
                {
                    ModelState.AddModelError("PhotoEvent", "The Date for StartTime needs to be before the Date of EndTime");
                    return Page();
                }
                else
                {
                    await _photoEventService.Add(PhotoEvent);
                }
            } 
            catch (UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccessDenied");
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
            await OnGet(PhotoEvent.ThePhotographer.ID);
            await OnGet(PhotoEvent.TheSchoolAdmin.ID);
            return RedirectToPage("/Index"); //return RedirectToPage("/Pages/PhotoEvents/Index"); 
        }
    }
}
