using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Students
{
    public class UpdateStudentModel : PageModel
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
        public UpdateStudentModel(IStudentService studentService, ISchoolService schoolService, ISchoolClassService schoolClassService, IParentServices parentServices)
        {
            _studentService = studentService;
            _schoolService = schoolService;
            _schoolClassService = schoolClassService;
            _parentServices = parentServices;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") == null)
                {
                    throw new Exception();
                }
                NewStudent = await _studentService.GetById(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
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
        public async Task<IActionResult> OnPostUpdate()
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
                await _studentService.Update(NewStudent);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("ClassGrade", "Class doesn't exist");
                //await OnGet();
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
