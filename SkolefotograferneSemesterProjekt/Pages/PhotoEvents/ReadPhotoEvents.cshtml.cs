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
        public List<PhotoEvent> PhotoEvents { get; set; }
        public ReadPhotoEventsModel(IPhotoEventService pEService)
        {
            PEService = pEService;
        }
        public async Task OnGet()
        {
            try
            {
                PhotoEvents = PhotoEventSort(await PEService.ShowActivePhotoEvent());
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
