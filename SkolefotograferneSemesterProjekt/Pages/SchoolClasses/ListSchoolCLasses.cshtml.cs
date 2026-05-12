using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolClasses
{
    public class ListSchoolCLassesModel : PageModel
    {
        #region Instance Fields
        private ISchoolClassService _schoolClassService;
        #endregion
        #region Properties
        [BindProperty]
        public IEnumerable<SchoolClass> SchoolClasses { get; set; }
        #endregion
        #region Constructor
        public ListSchoolCLassesModel(ISchoolClassService schoolClassService)
        {
            _schoolClassService = schoolClassService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 2)
                {
                    throw new UnauthorizedAccessException();
                }
                SchoolClasses = await _schoolClassService.GetAllByTeacher((int)HttpContext.Session.GetInt32("ID"));
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
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                await _schoolClassService.Delete(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage("Index");
        }
        #endregion
    }
}
