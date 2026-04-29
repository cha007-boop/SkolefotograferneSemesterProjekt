using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Photographers
{
    public class CreatePhotographerModel : PageModel
    {
        #region Instance fields
        private IPhotographerService photographerService;
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
            photographerService = service;
        }
        #endregion
        #region Methods
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if(NewPhotographer.Password == VerifyPassword)
                await photographerService.Add(NewPhotographer);
                else
                {
                    return Page();
                }
            }
            catch (TakenMailException texc)
            {
                ViewData["ErrorMessage"] = texc;
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
            return RedirectToPage("Photographers/index");
        }
        #endregion
    }
}
