using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers.Filter;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Pages.Photographers
{
    public class ShowPhotographersModel : PageModel
    {
        #region Instance fields
        private IPhotographerService photographerService;
        #endregion
        #region Properties
        [BindProperty]
        public IEnumerable<Photographer> Photographers { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FilterBy { get; set; }
        #endregion
        #region Constructor
        public ShowPhotographersModel(IPhotographerService service)
        {
            photographerService = service;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 4 || HttpContext.Session.GetInt32("Role") == 3)
                {
                    Photographers = Filter(await photographerService.GetAll());
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (UnauthorizedAccessException uax)
            {
                ViewData["ErrorMessage"] = uax.Message;
                return RedirectToPage("/Users/AccessDenied");
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }

        private IEnumerable<Photographer> Filter(IEnumerable<Photographer> photographers)
        {
            List<Predicate<Photographer>> predicates = new List<Predicate<Photographer>>();
            if (!string.IsNullOrWhiteSpace(FilterCriteria))
            {
                switch (FilterBy)
                {
                    case "All":
                        predicates.Add(b => b.FilterAll().Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "FirstName":
                        predicates.Add(b => !string.IsNullOrEmpty(b.FirstName) && b.FirstName.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "SurName":
                        predicates.Add(b => !string.IsNullOrEmpty(b.Surname) && b.Surname.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "PhoneNumber":
                        predicates.Add(b => !string.IsNullOrEmpty(b.PhoneNumber) && b.PhoneNumber.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "Mail":
                        predicates.Add(b => !string.IsNullOrEmpty(b.Email) && b.Email.Contains(FilterCriteria, StringComparison.OrdinalIgnoreCase));
                        break;
                    default:
                        break;
                }
            }
            return FilterFunctions<Photographer>.Filter(photographers, predicates);
        }
        #endregion
    }
}
