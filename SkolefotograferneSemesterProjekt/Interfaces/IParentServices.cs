using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IParentServices
    {
        /// <summary>
        /// Method for adding a parent to the database
        /// </summary>
        /// <param name="parent">The parent to add</param>
        /// <returns>A task representing the asynchronous operation, with the ID of the added parent as the result</returns>
        Task<int> AddParent(Parent parent);
        /// <summary>
        /// Method for retrieving all parents from the database
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all parents</returns>
        Task<List<Parent>> GetAllParents();
        /// <summary>
        /// Method for filtering parents based on a search string. The search string will be used to match against the parents' first name, surname, and phone number.
        /// </summary>
        /// <param name="Filter">The search string to filter parents by</param>
        /// <returns>A task representing the asynchronous operation, containing a list of parents that match the search criteria</returns>
        Task<List<Parent>> FilterParents(string Filter);

        /// <summary>
        /// Method for searching a parent by their ID.
        /// </summary>
        /// <param name="id">The ID of the parent to search for</param>
        /// <returns>A task representing the asynchronous operation, containing the parent if found</returns>
        Task<Parent> SearchParent(int id);

        /// <summary>
        /// Method for deleting a parent from the database.
        /// </summary>
        /// <param name="parent">The parent to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task DeleteParent(Parent parent);

        /// <summary>
        /// Method for updating a parent in the database.
        /// </summary>
        /// <param name="newParent">The updated parent. This should contain the new values for the parent's properties, and the ID should be the ID of the parent to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Update(Parent newParent);

    }
}
