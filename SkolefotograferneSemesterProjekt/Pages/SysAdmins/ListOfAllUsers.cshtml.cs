using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;

namespace SkolefotograferneSemesterProjekt.Pages.SysAdmins
{
    public class ListOfAllUsersModel : PageModel
    {
        private IUserService _userService;

        public ListOfAllUsersModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
        }
    }
}
