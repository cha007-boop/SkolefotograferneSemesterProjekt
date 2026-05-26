using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;

namespace SkolefotograferneSemesterProjekt.Pages.Users
{
    public class LoginModel : PageModel
    {
        private IUserService _userService;

        [BindProperty] 
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnGetLogOut()
        {
            HttpContext.Session.Remove("ID");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            return RedirectToPage("/Index");
        }
        
        public async Task<IActionResult> OnPost()
        {
            try
            {
                var user = await _userService.VerifyUser(Email, Password);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("ID", user.ID);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetInt32("Role", (int)user.Role);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Forkert email eller adgangskode.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Der opstod en fejl: {ex.Message}");
                return Page();
            }

        }

    }
}
