using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Method for adding a user login information to the database.
        /// </summary>
        /// <param name="conn">The SQL connection to use for the operation</param>
        /// <param name="user">The user to add</param>
        /// <returns>A task representing the asynchronous operation, containing the ID of the added user, so it can be used for further operations</returns>
        Task<int> Add(SqlConnection conn, User user);

        /// <summary>
        /// Method for deleting a user from the database.
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Delete(int id);
        /// <summary>
        /// Method validating that the user information provided for updating an existing user is valid, such as checking if the email is not already taken by another user, and if the password meets certain criteria.
        /// </summary>
        /// <param name="user">The user to validate</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task ValidateUpdate(User user);
        /// <summary>
        /// Method for getting all users from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all users</returns>
        Task<List<User>> GetAll();
        /// <summary>
        /// Method for verifying a user's login credentials.
        /// </summary>
        /// <param name="mail">The email of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A task representing the asynchronous operation, containing the user if the credentials are valid</returns>
        Task<User> VerifyUser(string mail, string password);
        /// <summary>
        /// Method for checking if an email is already taken by another user.
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <returns>A task representing the asynchronous operation, containing true if the email is taken, false otherwise</returns>
        Task<bool> IsEmailTaken(User user);
    }
}
