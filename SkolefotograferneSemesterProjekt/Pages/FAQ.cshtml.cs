using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages
{
    public class FAQModel : PageModel
    {
        [BindProperty]
        public List<string> Questions { get; set; } = [];
        [BindProperty]
        public bool IsAdmin { get; set; }
        [BindProperty]
        public string FileName { get; set; }

        public FAQModel()
        {
            FileName = "FAQtekst.txt";
        }

        public void OnGet()
        {
            int? role = HttpContext.Session.GetInt32("Role");
            if(role == (int)UserRole.SysAdmin)
            {
                IsAdmin = true;
            }

            try
            {
                string? temp = "";
                using (StreamReader reader = new StreamReader(FileName))
                {
                    
                    while ((temp += reader.ReadLine()) != null) 
                    {
                    }
                }
                Questions = temp.Split("|").ToList();
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
