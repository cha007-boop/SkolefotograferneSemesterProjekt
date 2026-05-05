using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoService
    {
        Task Add(Photo photo);
        Task<Photo> GetByFilename(string filename);
        Task<List<Photo>> GetAll();
        Task<List<Photo>> GetClassPhotosByClass(SchoolClass schoolClass);
        Task<List<Photo>> GetPortraitsByStudent(Student student);
        Task RemovePhoto(Photo photo);

    }
}
