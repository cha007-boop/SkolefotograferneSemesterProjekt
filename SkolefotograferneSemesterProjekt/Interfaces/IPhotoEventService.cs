using SkolefotograferneSemesterProjekt.Models;
using System.Runtime.CompilerServices;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoEventService
    {
        /// <summary>
        /// Method for adding a photoevent to the databse.
        /// </summary>
        /// <param name="photoEvent">The photo event to be added</param>
        /// <returns>A task representing the asynchronous operation, containing the ID of the added photo event</returns>
        Task<int> Add(PhotoEvent photoEvent);
        /// <summary>
        /// Method for getting all active photo events
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all active photo events</returns>
        Task<List<PhotoEvent>> ShowActivePhotoEvent();
        /// <summary>
        /// Method for searching photo events by photographer ID
        /// </summary>
        /// <param name="ID">The ID of the photographer</param>
        /// <returns>A task representing the asynchronous operation, containing a list of photo events for the specified photographer</returns>
        Task<List<PhotoEvent>> SearchEventByPhortographerID(int ID);
        /// <summary>
        /// Method for searching photo events by school admin ID
        /// </summary>
        /// <param name="ID">The ID of the school admin</param>
        /// <returns>A task representing the asynchronous operation, containing a list of photo events for the specified school admin</returns>
        Task<List<PhotoEvent>> SearchEventBySchoolAdminID(int ID);
        /// <summary>
        /// Method for getting all photo events
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all photo events</returns>
        Task<List<PhotoEvent>> GetAll();
        /// <summary>
        /// Method for getting a photo event by its ID
        /// </summary>
        /// <param name="id">The ID of the photo event</param>
        /// <returns>A task representing the asynchronous operation, containing the photo event with the specified ID</returns>
        Task<PhotoEvent?> GetByID(int id);
        /// <summary>
        /// Method for getting photo events for which a child of a given parent is signed up to
        /// </summary>
        /// <param name="parentId">ID of the parent of the child</param>
        /// <returns>A task representing the asynchronous operation, containing a list of photo events for the specified parent</returns>
        Task<List<PhotoEvent>> GetByParent(int parentId);
        /// <summary>
        /// Method for updating a photo event in the databse
        /// </summary>
        /// <param name="photoEvent">A PhotoEvent object containing new values, but the ID of the given photoevent must be the ID of the photoevent to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task UpdatePhotoEvent(PhotoEvent photoEvent);
        /// <summary>
        /// Method for removing a photo event from the database
        /// </summary>
        /// <param name="photoEvent">The photoevent to remove</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeletePhotoEvent(PhotoEvent photoEvent);
        /// <summary>
        /// Method for searching a photo event by its ID
        /// </summary>
        /// <param name="id">ID of the photoevent to find</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task<PhotoEvent> searchPhotoEvent(int id);
    }
}
