using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data.SqlTypes;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class UpdatePhotoEventModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;
        private IPhotographerService _photographerService;
        private IPhotoEventService _photoEventService;

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> Photographers { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> SchoolAdmins { get; set; }
        public UpdatePhotoEventModel(IPhotoEventService photoEventService, ISchoolAdminService schoolAdminService, IPhotographerService photographerService)
        {
            _schoolAdminService = schoolAdminService;
            _photographerService = photographerService;
            _photoEventService = photoEventService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                PhotoEvent = await _photoEventService.searchPhotoEvent(id);
                if(HttpContext.Session.GetInt32("Role") != 1 && HttpContext.Session.GetInt32("Role") != 3 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException();
                }
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
            catch (UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccessDenied");
            }
            catch(Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (PhotoEvent.StartTime > PhotoEvent.EndTime)
                {
                    ModelState.AddModelError("PhotoEvent", "The Date for StartTime needs to be before the Date of EndTime");
                    await OnGet(PhotoEvent.ID);
                    return Page();
                }
                if (PhotoEvent.StartTime < DateTime.Now)
                {
                    ModelState.AddModelError("PhotoEvent", "The Date for StartTime needs to be before the Date of EndTime");
                    await OnGet(PhotoEvent.ID);
                    return Page();
                }
                else
                {
                    await _photoEventService.UpdatePhotoEvent(PhotoEvent);
                }
            }
            catch (SqlException ex)
            {
                ViewData["ErrorMessage"] = ex;
                ModelState.AddModelError("PhotoEvent", ex.Message);
                await OnGet(PhotoEvent.ID);
                return Page();
            }
            catch (SqlTypeException tex)
            {
                ViewData["ErrorMessage"] = tex;
                ModelState.AddModelError("PhotoEvent.StartTime", tex.Message);
                await OnGet(PhotoEvent.ID);
                return Page();
            }
            return RedirectToPage("/PhotoEvents/ReadPhotoEvents");
        }
    }
}
