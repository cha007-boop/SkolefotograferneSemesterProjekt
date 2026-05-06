using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SysAdmins
{
    public class ListSysAdminsModel : PageModel
    {
        #region Instance fields
        private ISysAdminService _sysAdminService;
        #endregion
        #region Properties
        public List<SysAdmin> SysAdmins;
        #endregion
        #region Constructor
        public ListSysAdminsModel(ISysAdminService sysAdminService)
        {
            _sysAdminService = sysAdminService;
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
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }
        #endregion
    }
}
