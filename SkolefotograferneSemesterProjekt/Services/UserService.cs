using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class UserService : Connection, IUserService
    {
        public async Task<int> Add(SqlConnection conn, User user)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                string emailSearch = "Select Email from Users Where Email = @Email";
                SqlCommand command = new SqlCommand(emailSearch, connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Close();
                    throw new TakenMailException("Email is already used");
                }
            }
            
            try
            {

                var cmd = new SqlCommand(@"
                INSERT INTO Users (Email, Password, Role)
                VALUES (@Email, @Password, @Role);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ", conn);

                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
