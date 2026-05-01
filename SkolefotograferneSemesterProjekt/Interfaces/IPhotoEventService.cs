using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoEventService
    {
        Task Add(PhotoEvent photoEvent);
        Task<List<PhotoEvent>> ShowYourActivePhotoEvent();
    }
}
