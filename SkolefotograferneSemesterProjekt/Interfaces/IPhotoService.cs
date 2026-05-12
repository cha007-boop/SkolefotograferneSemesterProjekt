using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoService
    {
        Dictionary<string, string> SortableColumns { get; }
        Dictionary<string, string> FilterableColumns { get; }

        Task Add(Photo photo);
        Task<Photo> GetByFilename(string filename);
        Task<List<Photo>> GetAll();
        Task<List<Photo>> GetClassPhotosByClassId(int schoolClassId);
        Task<List<Photo>> GetPortraitsByStudentId(int studentId);
        Task<List<Photo>> GetByPhotoEventId(int photoEventId);
        Task RemovePhoto(Photo photo);
        Task<List<Photo>> Search(string filterColumn, string filterValue, string sortColumn, string sortOrder, List<string> conditions = null);
    }
}
