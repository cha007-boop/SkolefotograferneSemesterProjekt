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
        [BindProperty]
        public string PhotographerID { get; set; }
        [BindProperty]
        public string SchoolAdminID { get; set; }

        public CreatePhotoEventsModel(IPhotoEventService photoEventService, IPhotographerService photographerService, ISchoolAdminService schoolAdminService)
        {
            _photoEventService = photoEventService;
            _schoolAdminService = schoolAdminService;
            _photographerService = photographerService;
        }
        public async Task OnGet()
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
        public async Task<IActionResult> OnPost()
        {
            
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 3 || HttpContext.Session.GetInt32("Role") == 4)
                {
                    var id = HttpContext.Session.GetInt32("ID");
                    if (!id.HasValue)
                    {
                        ModelState.AddModelError("", "Session ID missing.");
                        await OnGet();
                        return Page();
                    }
                    if (HttpContext.Session.GetInt32("Role") == 3)
                    {
                        PhotoEvent.ThePhotographer = await _photographerService.SearchByID(Convert.ToInt32(PhotographerID));
                        PhotoEvent.TheSchoolAdmin = await _schoolAdminService.GetById(id.Value);
                        SchoolAdminID = id.Value.ToString();
                    } else if (HttpContext.Session.GetInt32("Role") == 4)
                    {
                        PhotoEvent.TheSchoolAdmin = await _schoolAdminService.GetById(Convert.ToInt32(SchoolAdminID));
                        PhotoEvent.ThePhotographer = await _photographerService.SearchByID(Convert.ToInt32(PhotographerID));
                    }
                    ModelState.Clear();
                    TryValidateModel(PhotoEvent);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
                if (PhotoEvent.StartTime > PhotoEvent.EndTime)
                {
                    ModelState.AddModelError("PhotoEvent.StartTime", "The Date for StartTime needs to be before the Date of EndTime");
                    await OnGet();
                    return Page();
                }
                if (PhotoEvent.StartTime == default)
                {
                    ModelState.AddModelError("PhotoEvent.StartTime", "Please set a StartTime");
                    await OnGet();
                    return Page();
                }
                if (PhotoEvent.EndTime == default)
                {
                    ModelState.AddModelError("PhotoEvent.EndTime", "Please set an EndTime");
                    await OnGet();
                    return Page();
                }
                if (PhotoEvent.StartTime < DateTime.Now)
                {
                    ModelState.AddModelError("PhotoEvent.StartTime", "The Date for StartTime needs to not be before the current todays date");
                    await OnGet();
                    return Page();
                } 
                if(PhotographerID == null)
                {
                    ModelState.AddModelError("PhotoEvent.ThePhotographer.ID", "Please choose a photographer");
                    await OnGet();
                    return Page();
                }
                if (SchoolAdminID == null)
                {
                    ModelState.AddModelError("PhotoEvent.TheSchoolAdmin.ID", "please choose a school admin");
                    await OnGet();
                    return Page();
                }
                if (PhotoEvent.Location == null)
                {
                    ModelState.AddModelError("PhotoEvent.Location", "please insert a location");
                    await OnGet();
                    return Page();
                }
                
                await _photoEventService.Add(PhotoEvent);
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
            return RedirectToPage("/Index", null); //return RedirectToPage("/Pages/PhotoEvents/Index"); 
        }
    }
}
