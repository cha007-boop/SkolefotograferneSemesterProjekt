using System.Data;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotographerService : Connection, IPhotographerService
    {
        #region Instance fields
        private IUserService userService;
        #endregion
        #region Constructor
        public PhotographerService()
        {
            userService = new UserService();
        }
        #endregion
        #region Methods
        public async Task<int> Add(Photographer photographer)
        {
            int userID = await userService.Add(photographer);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand sqlCommand = new SqlCommand(@"insert into Photographer 
                                         (ID, FirstName, Surname, PhoneNumber) 
                                         values (@ID, @FirstName, @Surname, @PhoneNumber)",
                                         connection);

                    sqlCommand.Parameters.AddWithValue("@ID", userID);
                    sqlCommand.Parameters.AddWithValue("@FirstName", photographer.FirstName);
                    sqlCommand.Parameters.AddWithValue("@Surname", photographer.Surname);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", photographer.PhoneNumber);

                    await sqlCommand.ExecuteNonQueryAsync();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw;
                }
            }
            return userID;
        }
        public async Task<List<Photographer>> GetAll()
        {
            List<Photographer> photographers = new List<Photographer>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select * from Photographer", connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    string? firstName = reader["FirstName"] as string;
                    string? surname = reader["Surname"] as string;
                    string? phoneNumber = reader["PhoneNumber"] as string;
                    string? website = reader["Website"] as string;
                    string? cVR = reader["CVR"] as string;
                    string? facebook = reader["Facebook"] as string;
                    string? instagram = reader["Instagram"] as string;
                    Photographer photographer = new Photographer { ID = id, FirstName = firstName, Surname = surname, PhoneNumber = phoneNumber, Website = website, CVR = cVR, Facebook = facebook, Instagram = instagram };
                    photographers.Add(photographer);
                }
                await reader.CloseAsync();
            }
            return photographers;
        }
        public async Task<Photographer> SearchByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select * from Users inner join Photographer on Users.ID = Photographer.ID where Users.ID = @ID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string? firstName = reader["FirstName"] as string;
                    string? surname = reader["Surname"] as string;
                    string? phoneNumber = reader["PhoneNumber"] as string;
                    string? website = reader["Website"] as string;
                    string? cVR = reader["CVR"] as string;
                    string? facebook = reader["Facebook"] as string;
                    string? instagram = reader["Instagram"] as string;
                    string? email = reader["Email"] as string;
                    Photographer photographer = new Photographer { ID = id, FirstName = firstName, Surname = surname, PhoneNumber = phoneNumber, Website = website, CVR = cVR, Facebook = facebook, Instagram = instagram, Email = email };
                    await reader.CloseAsync();
                    return photographer;
                }
                return null;
            }
        }
        public async Task Update(Photographer newPhotographer)
        {
            try
            {
                await userService.ValidateUpdate(newPhotographer);
                using SqlConnection conn = new SqlConnection(connectionString);
                {
                    SqlCommand command = new SqlCommand(@"UPDATE Users SET Email = @Email, Password = @Password WHERE ID = @ID", conn);
                    await command.Connection.OpenAsync();
                    command.Parameters.AddWithValue("@ID", newPhotographer.ID);
                    command.Parameters.AddWithValue("@Email", newPhotographer.Email);
                    command.Parameters.AddWithValue("@Password", newPhotographer.Password);
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = @"UPDATE Photographer SET FirstName = @FirstName, Surname = @Surname, PhoneNumber = @PhoneNumber, Website = @Website, CVR = @CVR, Facebook = @Facebook, Instagram = @Instagram WHERE ID = @ID";

                    command.Parameters.AddWithValue("@FirstName", newPhotographer.FirstName);
                    command.Parameters.AddWithValue("@Surname", newPhotographer.Surname);
                    command.Parameters.AddWithValue("@PhoneNumber", newPhotographer.PhoneNumber);
                    command.Parameters.AddWithValue("@Website", newPhotographer.Website);
                    command.Parameters.AddWithValue("@CVR", newPhotographer.CVR);
                    command.Parameters.AddWithValue("@Facebook", newPhotographer.Facebook);
                    command.Parameters.AddWithValue("@Instagram", newPhotographer.Instagram);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw;
            }
        }
        #endregion
    }
}
