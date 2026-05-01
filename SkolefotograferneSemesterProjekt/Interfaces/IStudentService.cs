using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IStudentService
    {
        Task Add(Student student);
        Task<Student> GetById(int id);
        Task<List<Student>> GetAll();
        Task<List<Student>> GetAllByParent(int parentID);//måske. kan nok bare bruge GetAll metoden og filtrere på den ud fra parentID...
        Task Update(Student student);
        Task Delete(int id);
    }
}
