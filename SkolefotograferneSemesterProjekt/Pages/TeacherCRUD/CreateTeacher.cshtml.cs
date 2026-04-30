using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Pages.TeacherCRUD
{
    public class CreateTeacherModel : PageModel
{
    private ITeacherService _repo;

    [BindProperty]
    public Teacher NewTeacher { get; set; }

    public CreateTeacherModel(ITeacherService repo)
    {
        _repo = repo;
    }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            await _repo.Add(NewTeacher);
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
            return Page();
        }
        return RedirectToPage("GetAllTeachers");
    }
}
}
