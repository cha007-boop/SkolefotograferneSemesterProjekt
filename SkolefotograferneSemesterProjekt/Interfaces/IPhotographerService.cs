using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    /// <summary>
    /// Provides methods for managing photographers.
    /// </summary>
    public interface IPhotographerService
    {

        /// <summary>
        /// Adds a new photographer.
        /// </summary>
        /// <param name="photographer">
        /// The photographer to add.
        /// </param>
        /// <returns>
        /// The ID of the newly created photographer.
        /// </returns>
        Task<int> Add(Photographer photographer);
        /// <summary>
        /// Retrieves all photographers.
        /// </summary>
        /// <returns>
        /// A list of all photographers.
        /// </returns>
        Task<List<Photographer>> GetAll();
        /// <summary>
        /// Searches for a photographer by ID.
        /// </summary>
        /// <param name="id">
        /// The ID of the photographer.
        /// </param>
        /// <returns>
        /// The photographer matching the specified ID.
        /// </returns>
        Task<Photographer> SearchByID(int id);
        /// <summary>
        /// Updates an existing photographer.
        /// </summary>
        /// <param name="newPhotographer">
        /// The photographer object containing updated information.
        /// </param>
        Task Update(Photographer newPhotographer);
    }
}
