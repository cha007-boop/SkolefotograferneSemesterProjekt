using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class UserService : Connection,IUserService
    {
        public async Task<int> Add(SqlConnection conn, User user)
        {
            var cmd = new SqlCommand(@"
                INSERT INTO Users (Email, Password, Role)
                VALUES (@Email, @Password, @Role);
                SELECT SCOPE_IDENTITY();
            ", conn);

            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Role", user.Role);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }

        public User VerifyUser(string mail, string password)
        {
            throw new NotImplementedException();
        }
    }
}
