using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SkolefotograferneSemesterProjekt.Pages.Users
{
    public class AccessDeniedModel : PageModel
    {
        public bool IsLoggedIn { get; set; }
        public void OnGet()
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            IsLoggedIn = userID.HasValue;
        }
    }
}
