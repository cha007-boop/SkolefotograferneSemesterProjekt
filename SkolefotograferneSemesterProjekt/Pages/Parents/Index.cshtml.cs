using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers.Sorting;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Threading.Tasks;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class IndexModel : PageModel
    {
        private IParentServices _parentService;
        public List<Parent> Parents { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public Student Student { get; set; }
        public School School { get; set; }

        public IndexModel(IParentServices parentservice)
        {
            _parentService = parentservice;
            SortOrder = "asc";
        }
        public async Task OnGet()
        {
            try
            {
                if (!string.IsNullOrEmpty(Filter))
                {
                    Parents = await _parentService.FilterParents(Filter);
                }else
                    Parents = await _parentService.GetAllParents();

                Parents = SortParents(Parents);
                
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
        }

        public string Toggle(string column)
        {
            if (SortOrder == "asc" && SortBy == column)
            {
                return "desc";
            }
            return "asc";
        }

        public List<Parent> SortParents(List<Parent> parents)
        {
            switch (SortBy)
            {
                case "FirstName":
                    parents.Sort(new ParentsCompareName());
                    break;
                case "Surname":
                    parents.Sort(new ParentsCompareSurname());
                    break;
                case "PhoneNumber":
                    parents.Sort(new ParentsComparePhoneNumber());
                    break;
                default:
                    break;

            }
            if (SortOrder != "asc") parents.Reverse();
            return parents;
        }


    }
}
