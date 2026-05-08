using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Photos
{
    public class ViewPhotosModel : PageModel
    {
        private IPhotoService _photoService;

        [BindProperty(SupportsGet =true)]
        public string FilterColumn { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterValue { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public List<Photo> Photos { get; set; } = new List<Photo>();

        public ViewPhotosModel(IPhotoService photoService)
        {
            _photoService = photoService;
            SortOrder = "ASC";
        }

        public async void OnGet()
        {

        }

        public string Toggle(string column)
        {
            //return (column == SortColumn) ? ("DESC") : "ASC";

            if (column == SortColumn && SortOrder == "ASC")
            {
                return "DESC";
            }
            return "ASC";
        }
    }
}
