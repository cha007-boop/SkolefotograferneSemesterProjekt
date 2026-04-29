using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolAdminService : Connection, ISchoolAdminService
    {
        private IUserService _userService = new UserService();

        public async Task Add(SchoolAdmin schoolAdmin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
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

        public Task<List<SchoolAdmin>> GetAll()
        {
            throw new NotImplementedException();
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
