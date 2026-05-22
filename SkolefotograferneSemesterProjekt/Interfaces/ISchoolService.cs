using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolService
    {
        /// <summary>
        /// All the columns the school table contains, with the key being the column name in the database, and the value being the display name for that column. This is used for filtering and sorting in the GetAll method.
        /// </summary>
        Dictionary<string, string> Columns { get; }
        /// <summary>
        /// Method for adding a school to the database.
        /// </summary>
        /// <param name="school">The school to add</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Add(School school);
        /// <summary>
        /// Method for getting a school by its ID.
        /// </summary>
        /// <param name="id">The ID of the school to get</param>
        /// <returns>A task representing the asynchronous operation, containing the school</returns>
        Task<School> GetById(int id);
        /// <summary>
        /// Method for getting all schools in the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all schools</returns>
        Task<List<School>> GetAll();
        /// <summary>
        /// Method for updating a school in the database.
        /// </summary>
        /// <param name="school">The updated school. This should contain the new values for the school's properties, and the ID should be the ID of the school to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Update(School school);
        /// <summary>
        /// Method for deleting a school from the database.
        /// </summary>
        /// <param name="id">The ID of the school to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Delete(int id);
        /// <summary>
        /// Method for getting all schools from the database, with optional filtering and sorting.
        /// </summary>
        /// <param name="filterColumn">The column to filter by</param>
        /// <param name="filterValue">The value to filter by</param>
        /// <param name="sortColumn">The column to sort by</param>
        /// <param name="sortOrder">The order to sort by (ASC or DESC)</param>
        /// <returns>A task representing the asynchronous operation, containing a list of all schools</returns>
        Task<List<School>> GetAll(string filterColumn, string filterValue, string sortColumn, string sortOrder);

    }
}
