using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Schools
{
    public class UpdateSchoolModel : PageModel
    {
        #region Instance fields
        private ISchoolService _schoolService;
        #endregion
        #region Properties
        [BindProperty]
        public School NewSchool { get; set; }
        #endregion
        #region Constructor
        public UpdateSchoolModel(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 4)
                {
                    NewSchool = await _schoolService.GetById(id);
                }
                else
                {
                    throw new AccessViolationException();
                }
            }
            catch (AccessViolationException avx)
            {
                ViewData["ErrorMessage"] = avx.Message;
                return Page();
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            try
            {
                await _schoolService.Update(NewSchool);
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return Page();
        }
        #endregion
    }
}
