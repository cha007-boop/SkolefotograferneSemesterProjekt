using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class UserService : Connection, IUserService
    {
        public async Task<int> Add(SqlConnection conn, User user)
        {
            if(user.Password.Length < 6)
            {
                throw new PasswordTooShortException("Password too short");
            }
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

        public async Task Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Users WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> VerifyUser(string mail, string password)
        {
            User foundUser = null;
             using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", mail);
                command.Parameters.AddWithValue("@Password", password);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        foundUser = new User
                        {
                            ID = reader.GetInt32("ID"),
                            Email = reader.GetString("Email"),
                            Password = reader.GetString("Password"),
                            Role = (UserRole)reader.GetInt32("Role")
                        };
                    }
                }
            }
            return foundUser;
        }
    }
}
