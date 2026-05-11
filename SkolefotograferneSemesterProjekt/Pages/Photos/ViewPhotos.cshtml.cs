using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Photos
{
    public class ViewPhotosModel : PageModel
    {
        private IPhotoService _photoService;

        public Dictionary<string, string> FilterableColumns
        {
            get { return _photoService.FilterableColumns; }
        }
        public Dictionary<string, string> SortableColumns
        {
            get { return _photoService.SortableColumns; }
        }

        [BindProperty(SupportsGet =true)]
        public string Type { get; set; }
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
            Type = "All";
        }

        public async Task OnGet()
        {
            Photos = await _photoService.Search(FilterColumn, FilterValue, SortColumn, SortOrder, Type);
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
