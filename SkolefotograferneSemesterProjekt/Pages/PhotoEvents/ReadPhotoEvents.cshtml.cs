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
        public List<PhotoEvent> PhotoEvents { get; set; }
        [BindProperty]
        public List<School> schools { get; set; }
        public ReadPhotoEventsModel(IPhotoEventService pEService)
        {
            PEService = pEService;
        }
        public async Task OnGet()
        {
            try
            { //recently changed
                PhotoEvents = PhotoEventSort(await PEService.SearchEventByPhortographerID((int)HttpContext.Session.GetInt32("ID"))).OrderBy(n => n.StartTime).ToList();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
        private List<PhotoEvent> PhotoEventSort(List<PhotoEvent> photoEvents)
        {
            return photoEvents;
        }
    }
}
