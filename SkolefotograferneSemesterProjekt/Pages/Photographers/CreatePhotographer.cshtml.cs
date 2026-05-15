using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Photographers
{
    public class CreatePhotographerModel : PageModel
    {
        #region Instance fields
        private IPhotographerService _photographerService;
        #endregion
        #region Properties
        [BindProperty]
        public Photographer NewPhotographer { get; set; }
        [BindProperty]
        public string VerifyPassword { get; set; }
        #endregion
        #region Constructor
        public CreatePhotographerModel(IPhotographerService service)
        {
            _photographerService = service;
        }
        #endregion
        #region Methods
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();
            TryValidateModel(NewPhotographer);
            try
            {
                if (NewPhotographer.Password == VerifyPassword)
                await _photographerService.Add(NewPhotographer);
                else
                {
                    ModelState.AddModelError("NewPhotographer.Password", "Password not the same");
                    return Page();
                }
                HttpContext.Session.SetInt32("ID", NewPhotographer.ID);
                HttpContext.Session.SetString("Email", NewPhotographer.Email);
                HttpContext.Session.SetInt32("Role", (int)NewPhotographer.Role);
            }
            catch (TakenMailException texc)
            {
                ViewData["ErrorMessage"] = texc;
                ModelState.AddModelError("NewPhotographer.Email", texc.Message);
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
                return Page();
            }
            return RedirectToPage("/Index");
        }
        #endregion
    }
}
