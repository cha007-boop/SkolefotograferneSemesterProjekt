using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IClassBookingService
    {
        Task<int> Book(SchoolClass schoolClass);
        Task<ClassBooking> GetByID(int id);
        Task Update(ClassBooking classBooking);
        Task Delete(ClassBooking classBooking);
    }
}
