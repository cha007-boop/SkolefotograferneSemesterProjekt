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
        private IUserService _userService;
        #endregion
        #region Properties
        [BindProperty]
        public Photographer Photographer { get; set; }
        #endregion
        #region Constructor
        public ViewPhotographerModel(IPhotographerService service, IUserService userService)
        {
            _photographerService = service;
            _userService = userService;
        }
        #endregion
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 4 || HttpContext.Session.GetInt32("Role") == 3)
                {

                    Photographer = await _photographerService.SearchByID(id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            try
            {
                await _userService.Delete(Photographer.ID);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage("/Photographers/ShowPhotographers");
        }
    }
}
