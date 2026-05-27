using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ITeacherService
    {
        /// <summary>
        /// Method for adding a teacher to the database.
        /// </summary>
        /// <param name="teacher">The teacher to add</param>
        /// <returns>A task representing the asynchronous operation, containing the ID of the added teacher</returns>
        Task<int> Add(Teacher teacher);
        /// <summary>
        /// Method for retrieving a teacher by their ID.
        /// </summary>
        /// <param name="id">The ID of the teacher to retrieve</param>
        /// <returns>A task representing the asynchronous operation, containing the teacher with the specified ID</returns>
        Task<Teacher?> GetByID(int id);
        /// <summary>
        /// Method for updating a teacher in the database
        /// </summary>
        /// <param name="teacher">A teacher object containing new values, but where the ID property is the same as the ID of the teacher to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Update(Teacher teacher);
        /// <summary>
        /// Method for deleting a teacher from the database
        /// </summary>
        /// <param name="teacher">The teacher to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Delete(Teacher teacher);
        /// <summary>
        /// Method for retrieving all teachers from the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all teachers</returns>
        Task<List<Teacher>> GetAll();
        /// <summary>
        /// Method for retrieving all teachers from the database that are associated with a specific school, identified by the school's ID
        /// </summary>
        /// <param name="id">The ID of the school</param>
        /// <returns>A task representing the asynchronous operation, containing a list of teachers associated with the specified school</returns>
        Task<List<Teacher>> GetBySchoolID(int id);
        
    }
}
