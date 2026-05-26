using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Photos
{
    public class ViewPhotosModel : PageModel
    {
        private IWebHostEnvironment _webHostEnvironment;
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

        public List<string> Conditions { get; set; } = new List<string>();

        public List<Photo> Photos { get; set; } = new List<Photo>();
        [BindProperty]
        public string? Message { get; set; } = null;

        public ViewPhotosModel(IPhotoService photoService, IWebHostEnvironment webHostEnvironment)
        {
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
            SortOrder = "ASC";
            Type = "All";
        }

        public async Task<IActionResult> OnGet()
        {
            int? role = HttpContext.Session.GetInt32("Role");
            int? id = HttpContext.Session.GetInt32("ID");
            if (role != 4 && role != 3 && role != 1)
            {
                return RedirectToPage("/Users/AccessDenied");
            }
            
            if (Type != "All")
            {
                if (Type == "ClassPhotos")
                {
                    Conditions.Add("ChildID IS NULL");
                }
                else if (Type == "Portraits")
                {
                    Conditions.Add("ChildID IS NOT NULL");
                }
            }

            if (role == 3 || role == 1)
            {
                
                Conditions.Add($"PhotoEventID IN (SELECT ID FROM PhotoEvent " +
                    $"WHERE PhotographerID = {id} OR SchoolAdminID IN " +
                    $"(SELECT ID FROM SchoolAdmin WHERE SchoolID IN (SELECT SchoolID FROM SchoolAdmin WHERE ID = {id})))");
            }

            
            Photos = await _photoService.Search(FilterColumn, FilterValue, SortColumn, SortOrder, Conditions);
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(string filename)
        {
            try
            {
                Photo photo = await _photoService.GetByFilename(filename);
                if (photo == null)
                {
                    return NotFound();
                }
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", photo.Child != null ? "Portraits" : "ClassPhotos", photo.Filename);
                System.IO.File.Delete(filePath);

                await _photoService.RemovePhoto(photo.Filename);
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("Message", $"Fejl ved sletning: {ex.Message}");
                
                Message = $"Fejl ved sletning: {ex.Message}";
                await OnGet();
                return Page();
            }
            await OnGet();
            return Page();
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
