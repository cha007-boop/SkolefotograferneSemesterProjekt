using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IClassBookingService
    {
        /// <summary>
        /// Method for adding a class booking to database
        /// </summary>
        /// <param name="classBooking">The class booking to be added</param>
        /// <returns>A task representing the asynchronous operation, containing the ID of the added class booking</returns>
        Task<int> Book(ClassBooking classBooking);
        /// <summary>
        /// Method for retrieving all class bookings from the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all class bookings</returns>
        Task<List<ClassBooking>> GetAll();
        /// <summary>
        /// Method for retrieving a class booking by its ID
        /// </summary>
        /// <param name="id">The ID of the class booking</param>
        /// <returns>A task representing the asynchronous operation, containing the class booking if found, otherwise null</returns>
        Task<ClassBooking?> GetByID(int id);
        /// <summary>
        /// Method for updating a class booking in the database
        /// </summary>
        /// <param name="classBooking">The class booking to be updated</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Update(ClassBooking classBooking);
        /// <summary>
        /// Method for deleting a class booking from the database
        /// </summary>
        /// <param name="classBooking">The class booking to be deleted</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Delete(ClassBooking classBooking);
        /// <summary>
        /// Method for retrieving class bookings by a specific teacher
        /// </summary>
        /// <param name="teacher">The teacher whose class bookings are to be retrieved</param>
        /// <returns>A task representing the asynchronous operation, containing a list of class bookings for the specified teacher</returns>
        Task<List<ClassBooking>> GetBookingsByTeacher(Teacher teacher);
        /// <summary>
        /// Method for retrieving class bookings by a specific photo event
        /// </summary>
        /// <param name="photoEvent">The photo event whose class bookings are to be retrieved</param>
        /// <returns>A task representing the asynchronous operation, containing a list of class bookings for the specified photo event</returns>
        Task<List<ClassBooking>> GetBookingsByPhotoEvent(PhotoEvent photoEvent);
        /// <summary>
        /// Method for checking if a class booking time is available
        /// </summary>
        /// <param name="classBooking">The class booking to be checked</param>
        /// <returns>A task representing the asynchronous operation, containing a bool indicating if the time is available</returns>
        Task<bool> IsTimeAvailable(ClassBooking classBooking);
    }
}
