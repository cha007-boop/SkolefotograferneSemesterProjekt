using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoEventService
    {
        Task<int> Add(PhotoEvent photoEvent);
        Task<List<PhotoEvent>> ShowActivePhotoEvent();
        Task<IEnumerable<PhotoEvent>> SearchEventByPhortographerID(int ID);
    }
}
