using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Schools
{
    public class GetAllSchoolModel : PageModel
    {
        private ISchoolService _schoolService;

        [BindProperty(SupportsGet = true)]
        public string FilterColumn { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterValue { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortColumn { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public Dictionary<string, string> FilterableColumns
        {
            get { return _schoolService.Columns; }
        }

        public List<School> Schools { get; set; }
        public GetAllSchoolModel(ISchoolService schoolService)
        {
            _schoolService = schoolService;
            SortOrder = "ASC";
        }

        public async Task OnGet()
        {
            try
            {
                //Schools = (string.IsNullOrWhiteSpace(FilterValue)) ? await _schoolService.GetAll() : await _schoolService.GetAll(FilterColumn, FilterValue, SortColumn, SortOrder);
                Schools = await _schoolService.GetAll(FilterColumn, FilterValue, SortColumn, SortOrder);
            }
            catch
            {

            }
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

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                await _schoolService.Delete(id);
            }
            catch(Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
            }
            await OnGet();
            return Page();
        }
    }
}
