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
        public async Task Add(Photographer photographer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    int userID = await userService.Add(connection, photographer);

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
        public Task Update(Photographer newPhotographer)
        {
            throw new NotImplementedException();
        }
        public async Task Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("delete from Photographer where ID = @ID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@ID", id);
                await command.ExecuteNonQueryAsync();
            }
        }
        #endregion
    }
}
