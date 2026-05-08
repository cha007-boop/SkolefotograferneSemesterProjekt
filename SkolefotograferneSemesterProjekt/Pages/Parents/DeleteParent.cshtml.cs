using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Security.Cryptography.X509Certificates;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class DeleteParentModel : PageModel
    {
        private IParentServices _parentService;
        public Parent ParentToDelete { get; set; }
        public DeleteParentModel(IParentServices parentService)
        {
            _parentService = parentService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") != 0 && HttpContext.Session.GetInt32("Role") != 4)
                {
                    throw new UnauthorizedAccessException("You do not have permission to access this page.");
                }
                    ParentToDelete = await _parentService.SearchParent(id);
                return Page();
            }
            catch (UnauthorizedAccessException aex)
            {
                ViewData["Errormessage"] = aex.Message;
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {

                return RedirectToPage("/Parents/ParentInformation", new { Id = ParentToDelete.ID });
            }
            catch (UnauthorizedAccessException aex)
            {
                ViewData["Errormessage"] = aex.Message;
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDelete()
        {
            try
            {
                ParentToDelete = await _parentService.SearchParent(ParentToDelete.ID);
                await _parentService.deleteParent(ParentToDelete);
                return RedirectToPage("/Parents/Index"); 
            }
            catch (UnauthorizedAccessException aex)
            {
                ViewData["Errormessage"] = aex.Message;
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
                return Page();
            }
        }

    }
}
