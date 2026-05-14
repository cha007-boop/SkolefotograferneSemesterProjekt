using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolClasses
{
    public class AddSchoolClassModel : PageModel
    {
        #region Instance fields
        private ISchoolClassService _schoolClassService;
        private ITeacherService _teacherService;
        #endregion
        #region Properties
        [BindProperty]
        public SchoolClass NewSchoolClass { get; set; }
        [BindProperty]
        public Teacher ThisTeacher { get; set; }
        #endregion
        #region Constructor
        public AddSchoolClassModel(ISchoolClassService schoolClassService, ITeacherService teacherService)
        {
            _schoolClassService = schoolClassService;
            _teacherService = teacherService;
        }
        #endregion
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 2)
                {
                    throw new UnauthorizedAccessException();
                }
                ThisTeacher = await _teacherService.GetByID((int)HttpContext.Session.GetInt32("ID"));
            }
            catch (UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccesDenied");
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            TryValidateModel(NewSchoolClass);
            try
            {
                NewSchoolClass.TheTeacher = ThisTeacher;
                NewSchoolClass.TheSchool = ThisTeacher.TheSchool;
                NewSchoolClass.SchoolYear = SchoolYearCalc.GetSchoolYear();
                if (NewSchoolClass.Grade < 11)
                    await _schoolClassService.Add(NewSchoolClass);
                else
                {
                    ModelState.AddModelError("NewSchoolClass", "Grade input incorrectly");
                    return Page();
                }
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
            }
            return RedirectToPage("ListSchoolClasses");
        }
    }
}
