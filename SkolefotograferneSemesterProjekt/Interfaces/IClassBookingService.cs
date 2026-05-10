using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IClassBookingService
    {
        Task<int> Book(ClassBooking classBooking);
        Task<List<ClassBooking>> GetAll();
        Task<ClassBooking?> GetByID(int id);
        Task Update(ClassBooking classBooking);
        Task Delete(ClassBooking classBooking);
        Task<List<ClassBooking>> GetBookingsByTeacher(Teacher teacher);
        Task<List<ClassBooking>> GetBookingsByPhotoEvent(PhotoEvent photoEvent);
        Task<bool> IsTimeAvailable(ClassBooking classBooking);
    }
}
