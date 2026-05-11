using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Students
{
    public class CreateStudentModel : PageModel
    {
        #region Instance fields
        private IStudentService _studentService;
        private ISchoolService _schoolService;
        private ISchoolClassService _schoolClassService;
        private IParentServices _parentServices;
        #endregion
        #region Properties
        [BindProperty]
        public Student NewStudent { get; set; }
        [BindProperty]
        public string SchoolID { get; set; }
        [BindProperty]
        public int ClassGrade { get; set; }
        [BindProperty]
        public string ClassLetter { get; set; }
        public IEnumerable<SelectListItem> Schools { get; set; }
        #endregion
        #region Constructor
        public CreateStudentModel(IStudentService service, ISchoolService schoolService, ISchoolClassService schoolClassService, IParentServices parentServices)
        {
            _studentService = service;
            _schoolService = schoolService;
            _schoolClassService = schoolClassService;
            _parentServices = parentServices;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") != 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Index");
            }
            List<School> schools = await _schoolService.GetAll();
            Schools = schools.Select(s => new SelectListItem
            {
                Value = Convert.ToString(s.ID),
                Text = $"{s.Name} - {s.Street} {s.ZipCode}"
            });
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            TryValidateModel(ClassGrade);
            try
            {
                if (ClassGrade > 10 || ClassGrade < 0)
                {
                    ModelState.AddModelError("ClassGrade", "Invalid Grade");
                    return Page();
                }
                string year = SchoolYearCalc.GetSchoolYear();
                NewStudent.TheSchoolClass = await _schoolClassService.SearchSchoolClass(Convert.ToInt32(SchoolID), ClassGrade, ClassLetter, year) ?? throw new ArgumentException();
                NewStudent.TheSchool = await _schoolService.GetById(Convert.ToInt32(SchoolID));
                NewStudent.TheParent = await _parentServices.SearchParent((int)HttpContext.Session.GetInt32("ID"));
                await _studentService.Add(NewStudent);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("ClassGrade", "Class doesn't exist");
                await OnGetAsync();
                return Page();
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc;
            }
            return RedirectToPage("Users/Profile");
        }
        #endregion
    }
}
