using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolClassService
    {
        Task Add(SchoolClass @class);
        Task<List<SchoolClass>> GetAll();
        Task<SchoolClass> GetByID(int id);
        Task<SchoolClass> SearchSchoolClass(int schoolID, int grade, string letter, string year);
        Task Update(SchoolClass newSchoolClass);
    }
}
