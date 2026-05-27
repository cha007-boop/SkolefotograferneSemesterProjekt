using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    /// <summary>
    /// Provides methods for managing school classes.
    /// </summary>
    public interface ISchoolClassService
    {
        /// <summary>
        /// Adds a new school class.
        /// </summary>
        /// <param name="class">
        /// The school class to add.
        /// </param>
        Task Add(SchoolClass @class);
        /// <summary>
        /// Retrieves all school classes.
        /// </summary>
        /// <returns>
        /// A list of all school classes.
        /// </returns>
        Task<List<SchoolClass>> GetAll();
        /// <summary>
        /// Retrieves a school class by its ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the school class.
        /// </param>
        /// <returns>
        /// The school class matching the specified ID.
        /// </returns>
        Task<SchoolClass> GetByID(int id);
        /// <summary>
        /// Searches for a school class using school ID, grade, letter, and year.
        /// </summary>
        /// <param name="schoolID">
        /// The ID of the school.
        /// </param>
        /// <param name="grade">
        /// The grade level of the class.
        /// </param>
        /// <param name="letter">
        /// The class letter designation.
        /// </param>
        /// <param name="year">
        /// The school year.
        /// </param>
        /// <returns>
        /// The matching school class.
        /// </returns>
        Task<SchoolClass> SearchSchoolClass(int schoolID, int grade, string letter, string year);
        /// <summary>
        /// Retrieves all school classes associated with a photo event.
        /// </summary>
        /// <param name="photoEventID">
        /// The ID of the photo event.
        /// </param>
        /// <returns>
        /// A list of school classes linked to the specified photo event.
        /// </returns>
        Task<List<SchoolClass>> GetByPhotoEvent(int photoEventID);
        /// <summary>
        /// Updates an existing school class.
        /// </summary>
        /// <param name="newSchoolClass">
        /// The school class object containing updated information.
        /// </param>
        Task Update(SchoolClass newSchoolClass);
        /// <summary>
        /// Retrieves all school classes assigned to a teacher.
        /// </summary>
        /// <param name="teacherid">
        /// The ID of the teacher.
        /// </param>
        /// <returns>
        /// A list of school classes assigned to the teacher.
        /// </returns>
        Task<List<SchoolClass>> GetAllByTeacher(int teacherid);
        /// <summary>
        /// Deletes a school class by ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the school class to delete.
        /// </param>
        Task Delete(int id);
        /// <summary>
        /// Retrieves all school classes belonging to a school.
        /// </summary>
        /// <param name="schoolID">
        /// The ID of the school.
        /// </param>
        /// <returns>
        /// A list of school classes for the specified school.
        /// </returns>
        Task<List<SchoolClass>> GetBySchool(int schoolID);
    }
}
