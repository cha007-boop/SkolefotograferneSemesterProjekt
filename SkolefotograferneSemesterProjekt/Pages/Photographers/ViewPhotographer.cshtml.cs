using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Pages.Photographers
{
    public class ViewPhotographerModel : PageModel
    {
        #region Instance fields
        private IPhotographerService photographerService;
        #endregion
        #region Properties
        //[BindProperty]
        public Photographer Photographer { get; set; }
        #endregion
        #region Constructor
        public ViewPhotographerModel(IPhotographerService service)
        {
            photographerService = service;
        }
        #endregion
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                Photographer = await photographerService.SearchByID(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
    }
}
