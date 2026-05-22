using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISchoolAdminService
    {
        /// <summary>
        /// All the columns the GetAll method can filter, with the key being the column name in the database, and the value being the display name for that column.
        /// </summary>
        Dictionary<string, string> FilterableColumns { get; }

        /// <summary>
        /// All the columns the GetAll method can sort by, with the key being the column name in the database, and the value being the display name for that column.
        /// </summary>
        Dictionary<string, string> SortableColumns { get; }

        /// <summary>
        /// Method for adding a school admin user to the database.
        /// </summary>
        /// <param name="schoolAdmin">The school admin to add</param>
        /// <returns>A task representing the asynchronous operation, containing the id of the added school admin</returns>
        Task<int> Add(SchoolAdmin schoolAdmin);

        /// <summary>
        /// Method for getting a school admin by their user ID.
        /// </summary>
        /// <param name="id">The user id to get by</param>
        /// <returns>A task representing the asynchronous operation, containing the school admin</returns>
        Task<SchoolAdmin> GetById(int id);

        /// <summary>
        /// Method for getting all school admin users from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all school admins</returns>
        Task<List<SchoolAdmin>> GetAll();

        /// <summary>
        /// Method for updating a school admin user in the database
        /// </summary>
        /// <param name="schoolAdmin">The updated school admin. This should contain the new values for the school admin's properties, and the ID should be the ID of the school admin to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Update(SchoolAdmin schoolAdmin);

        /// <summary>
        /// Method for getting all school admin users from the database, with optional filtering and sorting.
        /// </summary>
        /// <param name="filterColumn">The column to filter by</param>
        /// <param name="filterValue">The value to filter by</param>
        /// <param name="sortColumn">The column to sort by</param>
        /// <param name="sortOrder">The order to sort by (ASC or DESC)</param>
        /// <returns>A task representing the asynchronous operation, containing a list of all school admins</returns>
        Task<List<SchoolAdmin>> GetAll(string filterColumn, string filterValue, string sortColumn, string sortOrder);
    }
}
