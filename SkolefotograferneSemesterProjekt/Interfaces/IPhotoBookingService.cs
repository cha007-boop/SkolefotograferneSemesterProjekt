using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoBookingService
    {
        Task<int> Book(Teacher teacher);
        Task<ClassBooking> GetByID(int id);
        Task Update(ClassBooking classBooking);
        Task Delete(ClassBooking classBooking);
    }
}
