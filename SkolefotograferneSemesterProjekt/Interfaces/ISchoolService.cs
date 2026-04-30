using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolService
    {
        Task Add(School school);
        Task<School> GetById(int id);
        Task<List<School>> GetAll();
        Task Update(School school);
        Task Delete(int id);

    }
}
