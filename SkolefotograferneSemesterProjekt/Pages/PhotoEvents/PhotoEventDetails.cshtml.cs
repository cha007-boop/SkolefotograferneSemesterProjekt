using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.PhotoEvents
{
    public class PhotoEventDetailsModel : PageModel
    {
        private IPhotoEventService _photoEventService;
        private IClassBookingService _classBookingService;
        private ISchoolClassService _schoolClassService;
        private IStudentService _studentService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public PhotoEvent ThePhotoEvent { get; set; }

        [BindProperty(SupportsGet =true)]
        public string SelectedClassId { get; set; }
        

        public IEnumerable<SelectListItem> SchoolClassesSelectList { get; set; }
        public List<SchoolClass> SchoolClasses { get; set; }
        public List<Student> Students { get; set; }


        public PhotoEventDetailsModel(IPhotoEventService photoEventService, IClassBookingService classBookingService, ISchoolClassService schoolClassService, IStudentService studentService)
        {
            _photoEventService = photoEventService;
            _classBookingService = classBookingService;
            _schoolClassService = schoolClassService;
            _studentService = studentService;

            Students = new List<Student>();
            SchoolClasses = new List<SchoolClass>();
        }

        public async Task<IActionResult> OnGet()
        {

            ThePhotoEvent = await _photoEventService.GetByID(Id);
            if (ThePhotoEvent == null)
            {
                return NotFound();
            }

            SchoolClasses = await _schoolClassService.GetByPhotoEvent(Id);
            SchoolClassesSelectList = SchoolClasses.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = $"{c.Grade}{c.Letter}"
            });

            if (string.IsNullOrEmpty(SelectedClassId))
            {
                foreach (var schoolClass in SchoolClasses)
                {
                    var studentsInClass = await _studentService.GetByClass(schoolClass.ID);
                    Students.AddRange(studentsInClass);
                }
            }
            else
            {
                Students = await _studentService.GetByClass(Convert.ToInt32(SelectedClassId));
            }

            return Page();
        }

        
    }
}
