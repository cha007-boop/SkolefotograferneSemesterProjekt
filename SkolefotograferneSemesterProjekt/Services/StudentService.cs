using System.Data;
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

        public async Task<List<Student>> GetAllByParent(int parentID)
        {
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select * from Student where ParentID = @ParentID", connection);
                await command.Connection.OpenAsync();

                command.Parameters.AddWithValue("@ParentID", parentID);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    string firstName = reader.GetString("FirstName");
                    string surName = reader.GetString("Surname");
                    int schoolID = reader.GetInt32("SchoolID");
                    int classID = reader.GetInt32("ClassID");
                    Student student = new Student { ID = id, FirstName = firstName, Surname = surName, ParentID = parentID, SchoolID = schoolID, ClassID = classID };
                    students.Add(student);
                }
                await reader.CloseAsync();
            }
            return students;
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
