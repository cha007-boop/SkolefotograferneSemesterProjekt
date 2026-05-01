using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;

namespace SkolefotograferneSemesterProjekt.Pages.Users
{
    public class LoginModel : PageModel
    {
        [BindProperty] 
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        private IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public void OnGetLogOut()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserRole");
            RedirectToPage("/Index");
        }
        
        public async Task<IActionResult> OnPost()
        {
            try
            {
                var user = await _userService.VerifyUser(Email, Password);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.ID);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetInt32("UserRole", (int)user.Role);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return Page();
            }

        }

    }
}
