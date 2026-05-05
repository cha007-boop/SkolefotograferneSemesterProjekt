using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IStudentService
    {
        Task Add(Student student);
        Task<Student> GetById(int id);
        Task<List<Student>> GetAll();
        Task<List<Student>> GetAllByParent(int parentID);
        Task Update(Student student);
        Task Delete(int id);
        Task<List<Student>> GetByClass(int classID);
    }
}
