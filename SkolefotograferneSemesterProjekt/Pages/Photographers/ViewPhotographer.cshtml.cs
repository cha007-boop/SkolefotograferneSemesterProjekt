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
        private IPhotographerService _photographerService;
        #endregion
        #region Properties
        [BindProperty]
        public Photographer Photographer { get; set; }
        #endregion
        #region Constructor
        public ViewPhotographerModel(IPhotographerService service)
        {
            _photographerService = service;
        }
        #endregion
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                Photographer = await _photographerService.SearchByID(id);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            try
            {
                await _photographerService.Delete(Photographer.ID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage("Photographers/ShowPhotographers");
        }
    }
}
