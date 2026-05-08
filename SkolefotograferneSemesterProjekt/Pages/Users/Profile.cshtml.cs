using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Exceptions;
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
        private IStudentService _studentService;
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
        [BindProperty]
        public List<Student> Students { get; set; }
        [BindProperty]
        public string VerifyPassword { get; set; }
        #endregion
        #region Constructor
        public ProfileModel(IPhotographerService photographerService, IParentServices parentServices, ITeacherService teacherService, ISchoolAdminService schoolAdminService, ISysAdminService sysAdminService, IUserService userService, IStudentService studentService)
        {
            _photographerService = photographerService;
            _parentServices = parentServices;
            _teacherService = teacherService;
            _schoolAdminService = schoolAdminService;
            _sysAdminService = sysAdminService;
            _userService = userService;
            _studentService = studentService;
        }
        #endregion
        #region Methods
        public async Task<IActionResult> OnGet()
        {
            try
            {
                if (HttpContext.Session.GetInt32("Role") == 0)
                {
                    ThisParent = await _parentServices.SearchParent((int)HttpContext.Session.GetInt32("ID"));
                    Students = await _studentService.GetAllByParent((int)HttpContext.Session.GetInt32("ID"));
                }
                if (HttpContext.Session.GetInt32("Role") == 1)
                {
                    ThisPhotographer = await _photographerService.SearchByID((int)HttpContext.Session.GetInt32("ID"));
                }
                if (HttpContext.Session.GetInt32("Role") == 2)
                {
                    ThisTeacher = await _teacherService.GetByID((int)HttpContext.Session.GetInt32("ID"));
                }
                if (HttpContext.Session.GetInt32("Role") == 3)
                {
                    ThisSchoolAdmin = await _schoolAdminService.GetById((int)HttpContext.Session.GetInt32("ID"));
                }
                if (HttpContext.Session.GetInt32("Role") == 4)
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
                //if (!ModelState.IsValid)
                //{
                //    return Page();
                //}
                if (HttpContext.Session.GetInt32("Role") == 0)
                {
                    if(await UpdateCheckerAsync(ThisParent, "ThisParent"))
                    {
                        await _parentServices.Update(ThisParent);
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 1)
                {
                   if(await UpdateCheckerAsync(ThisPhotographer, "ThisPhotographer"))
                    {
                        await _photographerService.Update(ThisPhotographer);
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 2)
                {
                    if(await UpdateCheckerAsync(ThisTeacher, "ThisTeacher"))
                    {
                        await _teacherService.Update(ThisTeacher);
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 3)
                {
                    if(await UpdateCheckerAsync(ThisSchoolAdmin, "ThisSchoolAdmin"))
                    {
                        await _schoolAdminService.Update(ThisSchoolAdmin);
                    }
                }
                if (HttpContext.Session.GetInt32("Role") == 4)
                {
                    if (await UpdateCheckerAsync(ThisSysAdmin, "ThisSysAdmin"))
                    {
                        await _sysAdminService.Update(ThisSysAdmin);
                    }
                }
            }
            catch (InvalidMailException iexc)
            {
                ViewData["ErrorMessage"] = iexc;
                return Page();
            }
            catch (Exception exc)
            {
                ViewData["ErrorMessage"] = exc.Message;
                return Page();
            }
            return Page();
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
        private async Task<bool> UpdateCheckerAsync(User user, string thisuser)
        {
            if (user.Password == VerifyPassword && !await _userService.IsEmailTaken(user))
                return true;
            else
            {
                if (await _userService.IsEmailTaken(user))
                {
                    ModelState.AddModelError($"{thisuser}.Email", "Mailen er optaget");
                }
                if (user.Password != VerifyPassword)
                {
                    ModelState.AddModelError($"{thisuser}.Password", "Password not the same");
                }
                return false;
            }
        }
        #endregion
    }
}
