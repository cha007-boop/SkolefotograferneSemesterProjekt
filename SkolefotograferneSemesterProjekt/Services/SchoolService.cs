using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolService : Connection, ISchoolService
    {
        public async Task Add(School school)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    var command = new SqlCommand(@"
                    INSERT INTO School (Name, StudentCount, Street, ZipCode, Country)
                    VALUES (@Name, @StudentCount, @Street, @ZipCode, @Country)", conn);

                    command.Parameters.AddWithValue("@Name", school.Name);
                    command.Parameters.AddWithValue("@StudentCount", school.StudentCount);
                    command.Parameters.AddWithValue("@Street", school.Street);
                    command.Parameters.AddWithValue("@ZipCode", school.ZipCode);
                    command.Parameters.AddWithValue("@Country", school.Country);

                    await command.ExecuteNonQueryAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<School>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<School> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(School school)
        {
            throw new NotImplementedException();
        }
    }
}
