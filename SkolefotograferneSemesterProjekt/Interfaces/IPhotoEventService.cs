using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoEventService
    {
        Task Add(PhotoEvent photoEvent);
    }
}
