using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolAdminService : Connection, ISchoolAdminService
    {
        private IUserService _userService = new UserService();


        private string _getAllSql = "SELECT Users.ID, Users.Email, SchoolAdmin.PhoneNumber, SchoolAdmin.ContactPerson, SchoolAdmin.SchoolID " +
                                    "FROM Users INNER JOIN SchoolAdmin on Users.ID = SchoolAdmin.ID";



        public async Task Add(SchoolAdmin schoolAdmin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    int userID = await _userService.Add(conn, schoolAdmin);

                    var cmd = new SqlCommand(@"
                INSERT INTO SchoolAdmin (ID, PhoneNumber, ContactPerson, SchoolID)
                VALUES (@ID, @PhoneNumber, @ContactPerson, @SchoolID);
                ", conn);

                    cmd.Parameters.AddWithValue("@ID", userID);
                    cmd.Parameters.AddWithValue("@PhoneNumber", schoolAdmin.PhoneNumber);
                    cmd.Parameters.AddWithValue("@ContactPerson", schoolAdmin.ContactPerson);
                    cmd.Parameters.AddWithValue("@SchoolID", schoolAdmin.SchoolID);

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public async Task<List<SchoolAdmin>> GetAll()
        {
            List<SchoolAdmin> schoolAdmins = new List<SchoolAdmin>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(_getAllSql, conn);
                    await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        string email = reader.GetString("Email");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string contactPerson = reader.GetString("ContactPerson");
                        int schoolID = reader.GetInt32("SchoolID");

                        SchoolAdmin schoolAdmin = new SchoolAdmin { ID = id, Email = email, PhoneNumber = phoneNumber, ContactPerson = contactPerson, SchoolID = schoolID };
                        schoolAdmins.Add(schoolAdmin);
                    }
                    reader.Close();
                }
                catch
                {

                }
            }
            return schoolAdmins;
        }

        public Task<SchoolAdmin> GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(SchoolAdmin schoolAdmin)
        {
            throw new NotImplementedException();
        }
    }
}
