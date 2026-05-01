using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolService
    {
        Dictionary<string, string> FilterableColumns { get; }
        Task Add(School school);
        Task<School> GetById(int id);
        Task<List<School>> GetAll();
        Task Update(School school);
        Task Delete(int id);
        Task<List<School>> GetAll(string filterColumn, string filterValue, string sortColumn, string sortOrder);

    }
}
