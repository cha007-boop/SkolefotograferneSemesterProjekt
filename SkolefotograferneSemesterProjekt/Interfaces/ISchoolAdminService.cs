using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolAdminService
    {
        Task Add(SchoolAdmin schoolAdmin);
        Task<SchoolAdmin> GetByID(int id);
        Task<List<SchoolAdmin>> GetAll();
        Task Update(SchoolAdmin schoolAdmin);
        
    }
}
