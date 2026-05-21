using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
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
        public Teacher? ThisTeacher { get; set; }
        [BindProperty]
        public string Message { get; } = "Denne klasse findes allerede";
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
            }
            catch (UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccessDenied");
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
            int? userID = HttpContext.Session.GetInt32("ID");

            ModelState.Remove("Message");
            ModelState.CustomizedMessages("Feltet mangler");
            if (userID.HasValue)
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(NewSchoolClass?.Letter))
                    {
                        return Page();
                    }

                    string tempLetter = Regex.Replace(NewSchoolClass.Letter, "[0-9]", "");
                    if(tempLetter.Length <= 1)
                    {
                        tempLetter = tempLetter.ToUpper();
                    }
                    else
                    {
                        ModelState.AddModelError("NewSchoolClass.Letter", "Kun et bogstav må benyttes");
                        return Page();
                    }
                    NewSchoolClass.Letter = tempLetter;

                    ThisTeacher = await _teacherService.GetByID(userID.Value);
                    if (ThisTeacher == null)
                    {
                        return RedirectToPage("/Users/AccessDenied");
                    }
                    NewSchoolClass.TheTeacher = ThisTeacher;
                    NewSchoolClass.TheSchool = ThisTeacher.TheSchool;

                    int? schoolID = NewSchoolClass?.TheSchool?.ID;
                    if (!schoolID.HasValue)
                    {
                        return RedirectToPage("/Teachers/ViewSchoolClasses");
                    }

                    List<SchoolClass> classList = await _schoolClassService.GetBySchool(schoolID.Value);
                    SchoolClass? tempClass = classList.Find(c => c.Grade == NewSchoolClass.Grade && c.Letter == NewSchoolClass.Letter);

                    if (tempClass != null)
                    {
                        ModelState.AddModelError("Message", Message);
                        ModelState.AddModelError("NewSchoolClass.Grade", "Optaget");
                        ModelState.AddModelError("NewSchoolClass.Letter", "Optaget");
                        return Page();
                    }
                        NewSchoolClass.SchoolYear = SchoolYearCalc.GetSchoolYear();

                    if (NewSchoolClass.Grade >= 11)
                    {
                        ModelState.AddModelError("NewSchoolClass", "Grade input incorrectly");
                        return Page();
                    }
                    await _schoolClassService.Add(NewSchoolClass);
                }
                catch (Exception exc)
                {
                    ViewData["ErrorMessage"] = exc.Message;
                    return Page();
                }
                return RedirectToPage("/Teachers/ViewSchoolClasses", new { id = ThisTeacher.ID });
            }
            else
            {
                return RedirectToPage("/Users/AccessDenied");
            }
        }
        //OG
        //public async Task<IActionResult> OnPost()
        //{
        //    ModelState.Clear();
        //    TryValidateModel(NewSchoolClass);
        //    try
        //    {
        //        NewSchoolClass.TheTeacher = ThisTeacher;
        //        NewSchoolClass.TheSchool = ThisTeacher.TheSchool;
        //        NewSchoolClass.SchoolYear = SchoolYearCalc.GetSchoolYear();
        //        if (NewSchoolClass.Grade < 11)
        //            await _schoolClassService.Add(NewSchoolClass);
        //        else
        //        {
        //            ModelState.AddModelError("NewSchoolClass", "Grade input incorrectly");
        //            return Page();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        ViewData["ErrorMessage"] = exc.Message;
        //    }
        //    return RedirectToPage("ListSchoolClasses");
        //}
    }
}
