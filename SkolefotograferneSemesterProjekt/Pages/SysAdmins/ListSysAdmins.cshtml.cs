using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.SysAdmins
{
    public class ListSysAdminsModel : PageModel
    {
        #region Instance fields
        private ISysAdminService _sysAdminService;
        private IUserService _userService;
        #endregion
        #region Properties
        public List<SysAdmin> SysAdmins;
        #endregion
        #region Constructor
        public ListSysAdminsModel(ISysAdminService sysAdminService, IUserService userService)
        {
            _sysAdminService = sysAdminService;
            _userService = userService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 4)
                {
                    SysAdmins = await _sysAdminService.GetAll();
                }
                else
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
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                await _userService.Delete(id);

            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
        #endregion
    }
}
