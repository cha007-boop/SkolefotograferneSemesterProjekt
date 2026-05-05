using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IClassBookingService
    {
        Task<int> Book(ClassBooking classBooking, SchoolClass schoolClass, int photoEventID);
        Task<List<ClassBooking>> GetAll(int id);
        Task<ClassBooking> GetByID(int id);
        Task Update(ClassBooking classBooking);
        Task Delete(ClassBooking classBooking);
        Task<List<ClassBooking>> GetBookingsByTeacher(Teacher teacher);
        Task<List<ClassBooking>> GetBookingsByPhotoEvent(PhotoEvent photoEvent);
    }
}
