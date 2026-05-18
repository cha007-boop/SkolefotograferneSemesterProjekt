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

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> Photographers { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> SchoolAdmins { get; set; }
        public IEnumerable<SelectListItem> TimeSlots { get; set; } = [];

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
            await LoadMenus();
        }
        public async Task<IActionResult> OnPost(PhotoEvent photoEvent)
        {
            //ModelState.Clear();
            //TryValidateModel(PhotoEvent);
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 3 || HttpContext.Session.GetInt32("Role") == 4)
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
                if (PhotoEvent.StartTime > PhotoEvent.EndTime)
                {
                    ModelState.AddModelError(nameof(PhotoEvent.StartTime), "The Date for StartTime needs to be before the Date of EndTime");
                    return Page();
                }
                if (PhotoEvent.StartTime >= DateTime.Now)
                {
                    ModelState.AddModelError("PhotoEvent.StartTime", "The Date for StartTime needs to be before the Date of EndTime");
                    return Page();
                } 
                if(PhotoEvent.ThePhotographer.ID == null)
                {
                    ModelState.AddModelError(nameof(PhotoEvent.ThePhotographer.ID), "Please choose a photographer");
                    return Page();
                }
                if (PhotoEvent.TheSchoolAdmin == null)
                {
                    ModelState.AddModelError("PhotoEvent", "please choose a school admin");
                    return Page();
                }
                if (PhotoEvent.Location == null)
                {
                    ModelState.AddModelError("PhotoEvent", "please insert a location");
                    return Page();
                }
                else
                {
                    await _photoEventService.Add(PhotoEvent);
                }
                await LoadMenus();
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
            catch (NullReferenceException nrex)
            {
                ViewData["ErrorMessage"] = nrex.Message;
                return Page();
            }
            catch (SqlTypeException tex)
            {
                ViewData["ErrorMessage"] = tex;
                ModelState.AddModelError("PhotoEvent.StartTime", tex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            await OnGet(PhotoEvent.ThePhotographer.ID);
            await OnGet(PhotoEvent.TheSchoolAdmin.ID);
            return RedirectToPage("/Index", null); //return RedirectToPage("/Pages/PhotoEvents/Index"); 
        }
        private async Task LoadMenus()
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if (!userID.HasValue || PhotoEvent == null)
            {
                return;
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
            DateTime peCurrent = PhotoEvent.StartTime;
            DateTime peEnd = PhotoEvent.EndTime;
            List<SelectListItem> timeSlots = [];
            while (peCurrent.AddMinutes(20) <= peEnd)
            {
                PhotoEvent temp = new PhotoEvent() { StartTime = peCurrent };

                bool isAvailable = await _photoEventService.IsTimeAvailable(temp);
                if (isAvailable)
                {
                    timeSlots.Add(new SelectListItem
                    {
                        Value = peCurrent.ToString("dd/MM/yyyy HH:mm"),
                        Text = peCurrent.ToString("HH:mm")
                    });
                }
                peCurrent = peCurrent.AddMinutes(20);
            }

            TimeSlots = timeSlots;
        }
    }
}
