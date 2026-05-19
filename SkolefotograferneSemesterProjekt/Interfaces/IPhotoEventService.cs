using SkolefotograferneSemesterProjekt.Models;
using System.Runtime.CompilerServices;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoEventService
    {
        Task<int> Add(PhotoEvent photoEvent);
        Task<List<PhotoEvent>> ShowActivePhotoEvent();
        Task<List<PhotoEvent>> SearchEventByPhortographerID(int ID);
        Task<List<PhotoEvent>> SearchEventBySchoolAdminID(int ID);
        Task<List<PhotoEvent>> GetAll();
        Task<PhotoEvent?> GetByID(int id);
        Task<List<PhotoEvent>> GetByParent(int parentId);
        Task UpdatePhotoEvent(PhotoEvent photoEvent);
        Task DeletePhotoEvent(PhotoEvent photoEvent);
        Task<PhotoEvent> searchPhotoEvent(int id);
    }
}
