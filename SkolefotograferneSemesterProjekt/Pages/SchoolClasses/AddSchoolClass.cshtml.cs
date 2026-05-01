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
        public async Task OnGet()
        {
            try
            {
                ThisTeacher = await _teacherService.GetByID((int)HttpContext.Session.GetInt32("ID"));
                if (!HttpContext.Session.GetInt32("ID").HasValue)
                {
                    throw new Exception();
                }
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
            }
        }
        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            TryValidateModel(NewSchoolClass);
            try
            {
                NewSchoolClass.TeacherID = ThisTeacher.ID;
                NewSchoolClass.SchoolID = ThisTeacher.SchoolID;
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
            return RedirectToPage("Index");
        }
    }
}
