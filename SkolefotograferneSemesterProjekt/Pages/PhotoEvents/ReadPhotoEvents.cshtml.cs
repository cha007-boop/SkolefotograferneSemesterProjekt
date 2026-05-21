using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers.Filter;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class ReadPhotoEventsModel : PageModel
    {
        private IPhotoEventService PEService;

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public List<PhotoEvent> PhotoEvents { get; set; }
        [BindProperty]
        public bool PreviousPhotoEventsCheckBox { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? EventType { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }
        public ReadPhotoEventsModel(IPhotoEventService pEService)
        {
            PEService = pEService;
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 0)
                {
                    PhotoEvents = PhotoEventsFilter(await PEService.GetByParent((int)HttpContext.Session.GetInt32("ID"))).OrderBy(p => p.StartTime).ToList();
                }
                else if (HttpContext.Session.GetInt32("Role") == 1)
                {
                    PhotoEvents = PhotoEventsFilter(await PEService.SearchEventByPhortographerID((int)HttpContext.Session.GetInt32("ID"))).OrderBy(n => n.StartTime).ToList();
                } else if (HttpContext.Session.GetInt32("Role") == 3)
                {
                    PhotoEvents = PhotoEventsFilter(await PEService.SearchEventBySchoolAdminID((int)HttpContext.Session.GetInt32("ID"))).OrderBy(n => n.StartTime).ToList();
                }
                else if(HttpContext.Session.GetInt32("Role") == 4)
                {
                    //PhotoEvents = (await PEService.ShowActivePhotoEvent()).OrderBy(n => n.StartTime).ToList();
                    PhotoEvents = PhotoEventsFilter(await PEService.GetAll()).OrderBy(n => n.StartTime).ToList();
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch(UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccessDenied");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeletePhotoEvents()
        {
            await PEService.DeletePhotoEvent(PhotoEvent);
            return RedirectToPage();
        }

        private IEnumerable<PhotoEvent> PhotoEventsFilter(IEnumerable<PhotoEvent> photoEvents)
        {
            List<Predicate<PhotoEvent>> predicates = new List<Predicate<PhotoEvent>>();
            if (EventType != null)
            {
                if (EventType == 1)
                {
                    predicates.Add(p => p.StartTime > DateTime.Now);
                }
                else
                {
                    predicates.Add(p => p.StartTime < DateTime.Now);
                }
            }
            if (!string.IsNullOrWhiteSpace(FilterCriteria))
            {
                switch (FilterBy)
                {
                    case "ID":
                        predicates.Add(p => p.ID == Convert.ToInt32(FilterCriteria));
                        break;
                    case "Year":
                        predicates.Add(p => !string.IsNullOrEmpty(p.StartTime.Year.ToString()) && p.StartTime.Year.ToString().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "School":
                        predicates.Add(p => !string.IsNullOrEmpty(p.TheSchoolAdmin.TheSchool.Name) && p.TheSchoolAdmin.TheSchool.Name.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "Photographer":
                        predicates.Add(p => !string.IsNullOrEmpty(p.ThePhotographer.FirstName) && p.ThePhotographer.FirstName.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                        !string.IsNullOrEmpty(p.ThePhotographer.Surname) && p.ThePhotographer.Surname.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase) ||
                        !string.IsNullOrEmpty(p.ThePhotographer.FirstName + p.ThePhotographer.Surname) && (p.ThePhotographer.FirstName + " " + p.ThePhotographer.Surname).Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    default:
                        break;
                }
            }
            return FilterFunctions<PhotoEvent>.Filter(photoEvents, predicates);
        }
    }
}
