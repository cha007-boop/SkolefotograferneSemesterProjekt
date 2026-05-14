using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUserService
    {
        Task<int> Add(SqlConnection conn, User user);
        Task Delete(int id);
        Task ValidateUpdate(User user);
        Task<List<User>> GetAll();
        Task<User> VerifyUser(string mail, string password);
        Task<bool> IsEmailTaken(User user);
    }
}
