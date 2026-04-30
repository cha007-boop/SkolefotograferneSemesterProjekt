using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class CreateParentModel : PageModel
    {
        private IParentServices _parentservices;
        private IWebHostEnvironment _webHost;
        private IUserService _userService;

        [BindProperty]
        public Parent NewParent { get; set; }

        [BindProperty]
        public bool Consent { get; set; }

        [BindProperty]
        public string VerifyPassword { get; set; }

        public CreateParentModel(IParentServices parentService, IWebHostEnvironment webHost, IUserService userService)
        {
            _parentservices = parentService;
            _webHost = webHost;
            _userService = userService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            
            try
            {
                if (Consent)
                {
                    if (NewParent.Password == VerifyPassword)
                    {
                        if (NewParent.Email != null )
                        {
                            await _parentservices.AddParent(NewParent);
                        }
                    }
                    else
                    {
                        throw new Exceptions.PasswordNotTheSameException("Passwords are not the same");
                    }
                }
            }
            catch (TakenMailException ex)
            {
                ViewData["Errormessage"] = ex.Message;
                ModelState.AddModelError("Email", ex.Message);
                return Page();
            }
            catch (PasswordNotTheSameException ex)
            {
                ViewData["Errormessage"] = ex.Message;
                ModelState.AddModelError("VerifyPassword", ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Index");

        }
    }
}
