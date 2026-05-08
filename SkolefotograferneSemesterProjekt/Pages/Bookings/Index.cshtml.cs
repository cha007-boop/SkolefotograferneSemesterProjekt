using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        #region Instance fields
        private IClassBookingService _classBookingService;
        private ITeacherService _teacherService;
        private IPhotoEventService _photoEventService;
        #endregion
        #region Properties
        [BindProperty]
        public List<PhotoEvent> PhotoEvents { get; set; } = [];
        [BindProperty]
        public List<ClassBooking> Bookings { get; set; } = new();
        [BindProperty]
        public List<ClassBooking> BookingsAvailable { get; set; } = new(); 
        public Teacher ThisTeacher { get; set; }
        public int? UserID { get; set; }
        public bool IsUser { get; set; }
        #endregion
        #region Constructor
        public IndexModel(IClassBookingService classBookingService, ITeacherService teacherService, IPhotoEventService photoEventService)
        {
            _classBookingService = classBookingService;
            _teacherService = teacherService;
            _photoEventService = photoEventService;
        }
        #endregion
        #region Methods
        public async Task OnGet()
        {
            UserID = HttpContext.Session.GetInt32("ID");
            if (UserID != null)
            {
                Teacher t = await _teacherService.GetByID((int)UserID);
                if (t != null)
                {
                    IsUser = true;
                    ThisTeacher = t;
                }
            }

            PhotoEvents = await _photoEventService.GetAll();
            // Udkommenteret for debugging
            //foreach (PhotoEvent pe in await _photoEventService.GetAll())
            //{
            //    if (pe.StartTime > DateTime.Now)
            //    {
            //        PhotoEvents.Add(pe);
            //    }
            //}
        }
        #endregion
    }
}
