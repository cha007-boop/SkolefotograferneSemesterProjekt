using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Threading.Tasks;

namespace SkolefotograferneSemesterProjekt.Pages.Parents
{
    public class IndexModel : PageModel
    {
        private IParentServices _parentService;
        public List<Parent> Parents { get; set; }

        public IndexModel(IParentServices parentservice)
        {
            _parentService = parentservice;
        }
        public async Task OnGet()
        {
            try
            {
                Parents = await _parentService.GetAllParents();
            }
            catch (Exception ex)
            {
                ViewData["Errormessage"] = ex.Message;
            }
        }


    }
}
