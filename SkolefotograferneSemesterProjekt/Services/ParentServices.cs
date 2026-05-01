using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

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


        public async Task<List<Parent>> GetAllParents()
        {
            List<Parent> AllParents = new List<Parent>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"SELECT * FROM Parent INNER JOIN users ON Parent.ID = users.ID", conn);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int ID = reader.GetInt32("ID");
                    string FirstName = reader.GetString("FirstName");
                    string SurName = reader.GetString("Surname");
                    string PhoneNumber = reader.GetString("PhoneNumber");
                    string Email = reader.GetString("Email");
                    Parent parent = new Parent
                    {
                        ID = ID,
                        FirstName = FirstName,
                        Surname = SurName,
                        PhoneNumber = PhoneNumber, 
                        Email= Email,
                        Password = reader.GetString("Password"),
                        Role = UserRole.Parent

                    };
                    AllParents.Add(parent);
                }
                await reader.CloseAsync();
            }
            return AllParents;
        }
        public async Task<List<Parent>> FilterParents(string Filter)
        {
            List<Parent> parentsList = new List<Parent>();
            foreach (Parent p in await GetAllParents())
            {
                if (p.FirstName.ToLower().Contains(Filter.ToLower()))
                {
                    parentsList.Add(p);
                }
                else if (p.Surname.ToLower().Contains(Filter.ToLower())) 
                {
                    parentsList.Add(p);
                }
                else if (p.PhoneNumber.Contains(Filter))
                {
                    parentsList.Add(p);
                }
            }
            return parentsList;
        }




    }
}
