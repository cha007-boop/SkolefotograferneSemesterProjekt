using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Helpers;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Pages.Users
{
    public class ProfileModel : PageModel
    {
        #region Instance fields
        private IPhotographerService _photographerService;
        private IParentServices _parentServices;
        private ITeacherService _teacherService;
        private ISchoolAdminService _schoolAdminService;
        private ISysAdminService _sysAdminService;
        private IUserService _userService;
        #endregion
        #region Properties
        [BindProperty]
        public Photographer ThisPhotographer { get; set; }
        [BindProperty]
        public Parent ThisParent { get; set; }
        [BindProperty]
        public Teacher ThisTeacher { get; set; }
        [BindProperty]
        public SchoolAdmin ThisSchoolAdmin { get; set; }
        [BindProperty]
        public SysAdmin ThisSysAdmin { get; set; }
        #endregion
        #region Constructor
        public ProfileModel(IPhotographerService photographerService, IParentServices parentServices, ITeacherService teacherService, ISchoolAdminService schoolAdminService, ISysAdminService sysAdminService, IUserService userService)
        {
            _photographerService = photographerService;
            _parentServices = parentServices;
            _teacherService = teacherService;
            _schoolAdminService = schoolAdminService;
            _sysAdminService = sysAdminService;
            _userService = userService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if(HttpContext.Session.GetInt32("Role") == 0)
                {
                    ThisParent = await _parentServices.SearchParent((int)HttpContext.Session.GetInt32("ID"));
                }
                if(HttpContext.Session.GetInt32("Role") == 1)
                {
                    ThisPhotographer = await _photographerService.SearchByID((int)HttpContext.Session.GetInt32("ID"));
                }
                if(HttpContext.Session.GetInt32("Role") == 2)
                {
                    ThisTeacher = await _teacherService.GetByID((int)HttpContext.Session.GetInt32("ID"));
                }
                if(HttpContext.Session.GetInt32("Role") == 3)
                {
                    ThisSchoolAdmin = await _schoolAdminService.GetById((int)HttpContext.Session.GetInt32("ID"));
                }
                if(HttpContext.Session.GetInt32("Role") == 4)
                {
                    ThisSysAdmin = await _sysAdminService.SearchByID((int)HttpContext.Session.GetInt32("ID"));
                }
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostUpdate()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                if (HttpContext.Session.GetInt32("Role") == 0)
                {
                    ModelState.Remove("ThisParent.Password");
                    ModelState.CustomizedMessages("Feltet mangler");

                    if (await _userService.IsEmailTaken(ThisParent))
                    {
                        ModelState.AddModelError("ThisParent.Email", "Mailen er optaget");
                        return Page();
                    }
                    try
                    {
                        //await _parentServices.Update(ThisParent); //to be added in future
                    }
                    catch
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 1)
                {
                    ModelState.Remove("ThisPhotographer.Password");
                    ModelState.CustomizedMessages("Feltet mangler");

                    if (await _userService.IsEmailTaken(ThisPhotographer))
                    {
                        ModelState.AddModelError("ThisPhotographer.Email", "Mailen er optaget");
                        return Page();
                    }
                    try
                    {
                        await _photographerService.Update(ThisPhotographer);
                    }
                    catch
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 2)
                {
                    ModelState.Remove("ThisTeacher.Password");
                    ModelState.CustomizedMessages("Feltet mangler");

                    if (await _userService.IsEmailTaken(ThisTeacher))
                    {
                        ModelState.AddModelError("ThisTeacher.Email", "Mailen er optaget");
                        return Page();
                    }
                    try
                    {
                        await _teacherService.Update(ThisTeacher);
                    }
                    catch
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 3)
                {
                    ModelState.Remove("ThisSchoolAdmin.Password");
                    ModelState.CustomizedMessages("Feltet mangler");

                    if (await _userService.IsEmailTaken(ThisSchoolAdmin))
                    {
                        ModelState.AddModelError("ThisSchoolAdmin.Email", "Mailen er optaget");
                        return Page();
                    }
                    try
                    {
                        await _schoolAdminService.Update(ThisSchoolAdmin);
                    }
                    catch
                    {
                        throw;
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 4)
                {
                    ModelState.Remove("ThisSysAdmin.Password");
                    ModelState.CustomizedMessages("Feltet mangler");

                    if (await _userService.IsEmailTaken(ThisSysAdmin))
                    {
                        ModelState.AddModelError("ThisSysAdmin.Email", "Mailen er optaget");
                        return Page();
                    }
                    try
                    {
                        await _sysAdminService.Update(ThisSysAdmin);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                await _userService.Delete(id);
                HttpContext.Session.Remove("ID");
                HttpContext.Session.Remove("Email");
                HttpContext.Session.Remove("Role");
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return RedirectToPage("/Index");
        }
        #endregion
    }
}
