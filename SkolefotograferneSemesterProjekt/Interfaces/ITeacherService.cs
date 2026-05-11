using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ITeacherService
    {
        Task<int> Add(Teacher teacher);
        Task<Teacher?> GetByID(int id);
        Task Update(Teacher teacher);
        Task Delete(Teacher teacher);
        Task<List<Teacher>> GetAll();
        Task<List<Teacher>> GetBySchoolID(int id);
        
    }
}
