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

        public async Task<int> AddParent(Parent parent)
        {
            int userID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                {
                    await connection.OpenAsync();

                    userID = await userService.Add(connection, parent);
                    SqlCommand command = new SqlCommand(@"INSERT INTO Parent
                    (ID, FirstName, Surname, PhoneNumber) VALUES 
                    (@ID, @FirstName, @Surname, @PhoneNumber)", connection);
                   
                    command.Parameters.AddWithValue("@ID", userID);
                    command.Parameters.AddWithValue("@FirstName", parent.FirstName);
                    command.Parameters.AddWithValue("@Surname", parent.Surname);
                    command.Parameters.AddWithValue("@PhoneNumber", parent.PhoneNumber);

                    await command.ExecuteNonQueryAsync();
                }
            }
            return userID;
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
                else if(p.ToString().Contains(Filter, StringComparison.OrdinalIgnoreCase))
                {
                    parentsList.Add(p);
                }
            }
            return parentsList;
        }

        public async Task<Parent> SearchParent(int id)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(@"SELECT * FROM Parent INNER JOIN users ON Parent.ID = users.ID WHERE Parent.ID = @ID", conn);
                command.Parameters.AddWithValue("@ID", id);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
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
                        Email = Email,
                        Password = reader.GetString("Password"),
                        Role = UserRole.Parent
                    };
                    reader.Close();
                    return parent;
                }
                return null;
            }
        }

        public async Task deleteParent(Parent parent)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(@"DELETE FROM Users WHERE ID = @ID", conn);
                command.Parameters.AddWithValue("@ID", parent.ID);
                await command.Connection.OpenAsync();
                await command.ExecuteReaderAsync();
            }
        }

        public async Task Update(Parent newParent)
        {
            try
            {
                await userService.ValidateUpdate(newParent);
                using SqlConnection conn = new SqlConnection(connectionString);
                {
                    SqlCommand command = new SqlCommand(@"UPDATE Users SET Email = @Email WHERE ID = @ID", conn);
                    await command.Connection.OpenAsync();
                    command.Parameters.AddWithValue("@ID", newParent.ID);
                    command.Parameters.AddWithValue("@Email", newParent.Email);
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = @"UPDATE Parent SET FirstName = @FirstName, Surname = @Surname, PhoneNumber = @PhoneNumber WHERE ID = @ID";

                    command.Parameters.AddWithValue("@FirstName", newParent.FirstName);
                    command.Parameters.AddWithValue("@Surname", newParent.Surname);
                    command.Parameters.AddWithValue("@PhoneNumber", newParent.PhoneNumber);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw;
            }
        }
    }
}
