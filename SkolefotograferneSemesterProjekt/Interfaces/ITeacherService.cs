using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ITeacherService
    {
        Task<int> Add(Teacher teacher);
        Task<Teacher> GetByID(int id);
        Task<List<Teacher>> GetAll();
        Task Update(Teacher teacher);
        Task Delete(Teacher teacher);
        Task<bool> IsEmailTaken(Teacher teacher);
    }
}
