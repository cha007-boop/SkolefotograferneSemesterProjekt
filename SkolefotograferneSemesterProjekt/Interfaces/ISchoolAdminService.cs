using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolAdminService
    {
        Dictionary<string, string> FilterableColumns { get; }
        Dictionary<string, string> SortableColumns { get; }
        Task Add(SchoolAdmin schoolAdmin);
        Task<SchoolAdmin> GetById(int id);
        Task<List<SchoolAdmin>> GetAll();
        Task Update(SchoolAdmin schoolAdmin);
        Task<List<SchoolAdmin>> GetAll(string filterColumn, string filterValue, string sortColumn, string sortOrder);
    }
}
