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
        private ITeacherService _teacherService;
        private IPhotoEventService _photoEventService;
        private ISchoolAdminService _schoolAdminService;
        #endregion
        #region Properties
        public List<PhotoEvent> PhotoEvents { get; set; } = [];
        public List<PhotoEvent> FilteredList { get; set; } = [];
        public Teacher? ThisTeacher { get; set; }
        public SchoolAdmin? ThisSchoolAdmin { get; set; }
        public bool IsUser { get; set; }
        public int? Role { get; set; }
        #endregion
        #region Constructor
        public IndexModel(ITeacherService teacherService, IPhotoEventService photoEventService, ISchoolAdminService schoolAdminService)
        {
            _teacherService = teacherService;
            _photoEventService = photoEventService;
            _schoolAdminService = schoolAdminService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet()
        {
            PhotoEvents = await _photoEventService.GetAll();

            int? userID = HttpContext.Session.GetInt32("ID");
            Role = HttpContext.Session.GetInt32("Role");
            if(!userID.HasValue || !Role.HasValue)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            else
            {
                if (Role == (int)UserRole.Teacher)
                {
                    Teacher t = await _teacherService.GetByID(userID.Value);
                    if (t == null)
                    {
                        return RedirectToPage("/Users/AccessDenied");
                    }
                    IsUser = true;
                    ThisTeacher = t;

                    FilteredList = PhotoEvents.FindAll((pe) => pe.TheSchoolAdmin.TheSchool.ID == t.TheSchool.ID &&
                                                               pe.StartTime.Date >= DateTime.Today);
                }
                else if (Role == (int)UserRole.SchoolAdmin)
                {
                    SchoolAdmin sa = await _schoolAdminService.GetById(userID.Value);
                    if (sa == null)
                    {
                        return RedirectToPage("/Users/AccessDenied");
                    }
                    IsUser = false;
                    ThisSchoolAdmin = sa;

                    FilteredList = PhotoEvents.FindAll((pe) => pe.TheSchoolAdmin.TheSchool.ID == sa.TheSchool.ID &&
                                                               pe.StartTime.Date >= DateTime.Today);
                }
                else if (Role == (int)UserRole.SysAdmin)
                {
                    IsUser = false;
                    FilteredList = PhotoEvents.FindAll((pe) => pe.StartTime.Date >= DateTime.Today);
                }
                else
                {
                    return RedirectToPage("/Users/AccessDenied");
                }
            }
            return Page();
        }
        #endregion
    }
}
