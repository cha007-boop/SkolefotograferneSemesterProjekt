using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoService : IPhotoService
    {
        public Task Add(Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Photo> GetByFilename(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetClassPhotosByClass(SchoolClass schoolClass)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetPortraitsByStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Task RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
