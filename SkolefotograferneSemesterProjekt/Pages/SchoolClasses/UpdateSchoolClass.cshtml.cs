using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using static System.Net.Mime.MediaTypeNames;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolClasses
{
    public class UpdateSchoolClassModel : PageModel
    {
        #region Instance Fields
        private ISchoolClassService _schoolClassService;
        #endregion
        #region Properties
        [BindProperty]
        public SchoolClass NewSchoolClass { get; set; }
        #endregion
        #region Constructor
        public UpdateSchoolClassModel(ISchoolClassService schoolClassService)
        {
            _schoolClassService = schoolClassService;
        }
        #endregion
        #region Methods
        public async Task OnGet(int id)
        {
            try
            {
                NewSchoolClass = await _schoolClassService.GetByID(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            TryValidateModel(NewSchoolClass);
            try
            {
                if(NewSchoolClass.Grade > 10)
                {
                    ModelState.AddModelError("NewSchoolClass.Grade", "Grade cannot exceed 10");
                    return Page();
                }
                if(NewSchoolClass.Letter.Length > 1)
                {
                    ModelState.AddModelError("NewSchoolClass.Letter", "Letter cannot contain multiple letters");
                    return Page();
                }
                if(DateTime.Now.Year.ToString() != NewSchoolClass.SchoolYear || DateTime.Now.Year+1.ToString() != NewSchoolClass.SchoolYear)
                {
                    ModelState.AddModelError("NewSchoolClass.SchoolYear", "School year can only be edited to current or next year");
                    return Page();
                }
                await _schoolClassService.Update(NewSchoolClass);
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
            }
            return RedirectToPage("SchoolClasses/ListSchoolClasses");
        }
        #endregion
    }
}
