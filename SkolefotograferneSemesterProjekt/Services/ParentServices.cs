using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class ParentServices : Connection, IParentServices
    {
        private IUserService userService;
        public ParentServices()
        {
            userService = new UserService();
        }

        public async Task AddParent(Parent parent)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                {
                    await connection.OpenAsync();

                    int UserID = await userService.Add(connection, parent);
                    SqlCommand command = new SqlCommand(@"INSERT INTO Parent
            (ID, FirstName, Surname, PhoneNumber) VALUES 
            (@ID, @FirstName, @Surname, @PhoneNumber)", connection);
                   
                    command.Parameters.AddWithValue("@ID", UserID);
                    command.Parameters.AddWithValue("@FirstName", parent.FirstName);
                    command.Parameters.AddWithValue("@Surname", parent.Surname);
                    command.Parameters.AddWithValue("@PhoneNumber", parent.PhoneNumber);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
