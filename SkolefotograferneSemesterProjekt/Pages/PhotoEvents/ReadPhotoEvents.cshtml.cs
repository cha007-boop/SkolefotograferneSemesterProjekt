using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class ReadPhotoEventsModel : PageModel
    {
        private IPhotoEventService PEService;
        private IHttpContextAccessor HttpContextAccessor;

        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public List<PhotoEvent> PhotoEvents { get; set; }
        public ReadPhotoEventsModel(IPhotoEventService pEService, IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
            PEService = pEService;
        }
        public async Task OnGet()
        {
            try
            {
                if (HttpContextAccessor.HttpContext.Session.GetInt32("Role") == 1)
                {
                    PhotoEvents = PhotoEventSort(await PEService.SearchEventByPhortographerID((int)HttpContext.Session.GetInt32("ID"))).OrderBy(n => n.StartTime).ToList();
                } else if (HttpContextAccessor.HttpContext.Session.GetInt32("Role") == 3)
                {
                    PhotoEvents = PhotoEventSort(await PEService.SearchEventBySchoolAdminID((int)HttpContext.Session.GetInt32("ID"))).OrderBy(n => n.StartTime).ToList();
                }
                else if(HttpContextAccessor.HttpContext.Session.GetInt32("Role") == 4)
                {
                    PhotoEvents = PhotoEventSort(await PEService.ShowActivePhotoEvent()).OrderBy(n => n.StartTime).ToList();
                }
               
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
        public async Task<IActionResult> OnPostDeletePhotoEvents()
        {
            await PEService.DeletePhotoEvent(PhotoEvent);
            return RedirectToPage();
        }
        private List<PhotoEvent> PhotoEventSort(List<PhotoEvent> photoEvents)
        {
            return photoEvents;
        }
    }
}
