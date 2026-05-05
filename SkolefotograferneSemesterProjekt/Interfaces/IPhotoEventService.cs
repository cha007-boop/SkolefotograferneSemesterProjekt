using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoEventService
    {
        Task<int> Add(PhotoEvent photoEvent);
        Task<List<PhotoEvent>> ShowActivePhotoEvent();
        Task<List<PhotoEvent>> SearchEventByPhortographerID(int ID);
        Task<List<PhotoEvent>> GetAll();
        Task<PhotoEvent?> GetByID(int id);
    }
}
