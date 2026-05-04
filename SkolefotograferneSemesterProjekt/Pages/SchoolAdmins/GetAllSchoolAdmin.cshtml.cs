using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.SchoolAdmins
{
    public class GetAllSchoolAdminModel : PageModel
    {
        private ISchoolAdminService _schoolAdminService;

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
            get { return _schoolAdminService.FilterableColumns; }
        }
        public Dictionary<string, string> SortableColumns
        {
            get { return _schoolAdminService.SortableColumns; }
        }

        public List<SchoolAdmin> SchoolAdmins { get; set; }

        public GetAllSchoolAdminModel(ISchoolAdminService schoolAdminService)
        {
            _schoolAdminService = schoolAdminService;
            SortOrder = "ASC";
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                //SchoolAdmins = await _schoolAdminService.GetAll();
                SchoolAdmins = await _schoolAdminService.GetAll(FilterColumn, FilterValue, SortColumn, SortOrder);
            }
            catch
            {

            }
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
