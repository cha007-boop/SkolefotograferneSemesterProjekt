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
        [BindProperty]
        public PhotoEvent PhotoEvent { get; set; }
        [BindProperty]
        public List<PhotoEvent> PhotoEvents { get; set; }
        public ReadPhotoEventsModel(IPhotoEventService pEService)
        {
            PEService = pEService;
        }
        public async Task OnGet()
        {
            try
            {
                PhotoEvents = PhotoEventSort(await PEService.SearchEventByPhortographerID((int)HttpContext.Session.GetInt32("ID"))).OrderBy(n => n.StartTime).ToList();
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
