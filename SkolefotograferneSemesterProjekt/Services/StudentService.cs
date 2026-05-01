using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class StudentService : Connection, IStudentService
    {
        public async Task Add(Student student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand("insert into Student (FirstName, Surname, ParentID, SchoolID, ClassID) values (@FirstName, @Surname, @ParentID, @SchoolID, @ClassID)", connection);

                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@Surname", student.Surname);
                    command.Parameters.AddWithValue("@ParentID", student.ParentID);
                    command.Parameters.AddWithValue("@SchoolID", student.SchoolID);
                    command.Parameters.AddWithValue("@ClassID", student.ClassID);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw;
                }
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAllByParent(int parentID)
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
