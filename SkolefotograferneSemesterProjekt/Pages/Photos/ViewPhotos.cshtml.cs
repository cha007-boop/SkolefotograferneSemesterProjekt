using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;

namespace SkolefotograferneSemesterProjekt.Pages.Photos
{
    public class ViewPhotosModel : PageModel
    {
        private IPhotoService _photoService;

        public void OnGet()
        {
        }
    }
}
