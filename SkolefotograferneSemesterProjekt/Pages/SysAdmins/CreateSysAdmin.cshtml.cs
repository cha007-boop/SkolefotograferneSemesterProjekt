using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using static System.Net.Mime.MediaTypeNames;

namespace SkolefotograferneSemesterProjekt.Pages.SysAdmins
{
    public class CreateSysAdminModel : PageModel
    {
        #region Instance fields
        private ISysAdminService _sysAdminService;
        #endregion
        #region Properties
        [BindProperty]
        public SysAdmin NewSysAdmin { get; set; }
        [BindProperty]
        public string VerifyPassword { get; set; }
        [BindProperty]
        public string VerifyEmail { get; set; }
        #endregion
        #region Constructor
        public CreateSysAdminModel(ISysAdminService service)
        {
            _sysAdminService = service;
        }
        #endregion
        #region Methods
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            TryValidateModel(NewSysAdmin);
            try
            {
                if (NewSysAdmin.Email != VerifyEmail && NewSysAdmin.Password != VerifyPassword)
                {
                    ModelState.AddModelError("NewSysAdmin.Email", "Email er ikke ens");
                    ModelState.AddModelError("NewSysAdmin.Password", "Password er ikke ens");
                    return Page();
                }
                if (NewSysAdmin.Email != VerifyEmail)
                {
                    ModelState.AddModelError("NewSysAdmin.Email", "Email er ikke ens");
                    return Page();
                }
                if (NewSysAdmin.Password != VerifyPassword)
                {
                    ModelState.AddModelError("NewSysAdmin.Password", "Password er ikke ens");
                    return Page();
                }
                else
                    await _sysAdminService.Add(NewSysAdmin);
            }
            catch(PasswordTooShortException pexc)
            {
                ViewData["ErrorMessage"] = pexc.Message;
                ModelState.AddModelError("NewSysAdmin.Password", pexc.Message);
                return Page();
            }
            catch (TakenMailException texc)
            {
                ViewData["ErrorMessage"] = texc;
                ModelState.AddModelError("NewSysAdmin.Email", texc.Message);
                return Page();
            }
            catch (InvalidMailException iexc)
            {
                ViewData["ErrorMessage"] = iexc;
                return Page();
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc;
            }
            return RedirectToPage("index");
        }
        #endregion
    }
}
