using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    /// <summary>
    /// Provides methods for managing students.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">
        /// The student to add.
        /// </param>
        Task Add(Student student);
        /// <summary>
        /// Retrieves a student by ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the student.
        /// </param>
        /// <returns>
        /// The student matching the specified ID.
        /// </returns>
        Task<Student> GetById(int id);
        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <returns>
        /// A list of all students.
        /// </returns>
        Task<List<Student>> GetAll();
        /// <summary>
        /// Retrieves all students associated with a parent.
        /// </summary>
        /// <param name="parentID">
        /// The ID of the parent.
        /// </param>
        /// <returns>
        /// A list of students belonging to the specified parent.
        /// </returns>
        Task<List<Student>> GetAllByParent(int parentID);
        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">
        /// The student object containing updated information.
        /// </param>
        Task Update(Student student);
        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the student to delete.
        /// </param>
        Task Delete(int id);
        /// <summary>
        /// Retrieves all students belonging to a class.
        /// </summary>
        /// <param name="classID">
        /// The ID of the class.
        /// </param>
        /// <returns>
        /// A list of students in the specified class.
        /// </returns>
        Task<List<Student>> GetByClass(int classID);
    }
}
