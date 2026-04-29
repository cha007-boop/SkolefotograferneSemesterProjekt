using System.Data;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotographerService : Connection, IPhotographerService
    {
        #region Instance fields
        private UserService userService;
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
                    
                    SqlCommand sqlCommand = new SqlCommand(@"insert into Photographer (ID, FirstName, Surname, PhoneNumber) values (@ID, @FirstName, @Surname, @PhoneNumber)", connection);

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
                    string firstName = reader.GetString("FirstName");
                    string surname = reader.GetString("Surname");
                    string phoneNumber = reader.GetString("PhoneNumber");
                    string website = reader.GetString("Website");
                    string cVR = reader.GetString("CVR");
                    string facebook = reader.GetString("Facebook");
                    string instagram = reader.GetString("Instagram");
                    Photographer photographer = new Photographer { FirstName = firstName, Surname = surname, PhoneNumber = phoneNumber, Website = website, CVR = cVR, Facebook = facebook, Instagram = instagram };
                    photographers.Add(photographer);
                }
                await reader.CloseAsync();
            }
            return photographers;
        }
        public Task Update(Photographer newPhotographer)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
