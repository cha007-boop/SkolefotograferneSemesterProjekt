using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Users
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public User user { get; set; }
        public SignUpModel()
        {
            
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {

        }
    }
}
