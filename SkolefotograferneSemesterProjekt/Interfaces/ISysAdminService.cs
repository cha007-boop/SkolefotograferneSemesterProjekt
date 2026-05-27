using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    /// <summary>
    /// Provides methods for managing system administrators.
    /// </summary>
    public interface ISysAdminService
    {
        /// <summary>
        /// Adds a new system administrator.
        /// </summary>
        /// <param name="sysAdmin">
        /// The system administrator to add.
        /// </param>
        /// <returns>
        /// The ID of the newly created system administrator.
        /// </returns>
        Task<int> Add(SysAdmin sysAdmin);
        /// <summary>
        /// Retrieves all system administrators.
        /// </summary>
        /// <returns>
        /// A list of all system administrators.
        /// </returns>
        Task<List<SysAdmin>> GetAll();
        /// <summary>
        /// Searches for a system administrator by ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the system administrator.
        /// </param>
        /// <returns>
        /// The system administrator matching the specified ID.
        /// </returns>
        Task<SysAdmin> SearchByID(int id);
        /// <summary>
        /// Updates an existing system administrator.
        /// </summary>
        /// <param name="newSysAdmin">
        /// The system administrator object containing updated information.
        /// </param>
        Task Update(SysAdmin newSysAdmin);
    }
}
