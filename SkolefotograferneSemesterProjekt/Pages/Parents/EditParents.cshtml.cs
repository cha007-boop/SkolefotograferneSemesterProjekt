using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class EditParentsModel : PageModel
    {
        private IParentServices _parentService;
        [BindProperty]
        public Parent NewParent { get; set; }
        [BindProperty]
        public string VerifyPassword { get; set; }
        public EditParentsModel(IParentServices parentService)
        {
            _parentService = parentService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 0 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this page.");
                }
                NewParent = await _parentService.SearchParent(id);
            }
            catch (UnauthorizedAccessException ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
            return Page();
        }
        public async Task<IActionResult> onPost()
        {
            if (NewParent.Password == VerifyPassword)
            {
                if (NewParent.Email != null)
                {
                    await _parentService.Update(NewParent);
                }
            }
            else
            {
                throw new Exceptions.PasswordNotTheSameException("Passwords are not the same");
            }
            return Page();
        }
    }
}
