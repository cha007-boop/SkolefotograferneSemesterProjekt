using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUserService
    {
        Task<int> Add(SqlConnection conn, User user);
        Task Delete(int id);
        Task Update(User user);

        Task<User> VerifyUser(string mail, string password);
    }
}
