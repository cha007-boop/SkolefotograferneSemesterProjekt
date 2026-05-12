using System.Data;
using System.Security.Cryptography.Xml;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class StudentService : Connection, IStudentService
    {
        private IParentServices _parentService = new ParentServices();
        private ISchoolService _schoolService = new SchoolService();
        private ISchoolClassService _schoolClassService = new SchoolClassService();

        public async Task Add(Student student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand("insert into Student (FirstName, Surname, ParentID, ClassID) values (@FirstName, @Surname, @ParentID, @ClassID)", connection);

                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@Surname", student.Surname);
                    command.Parameters.AddWithValue("@ParentID", student.TheParent.ID);
                    command.Parameters.AddWithValue("@ClassID", student.TheSchoolClass.ID);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw;
                }
            }
        }

        public async Task Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("delete from Student where ID = @ID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@ID", id);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Student>> GetAll()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select * from Student", connection);
                await command.Connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    string firstName = reader.GetString("FirstName");
                    string surName = reader.GetString("Surname");
                    int parentID = reader.GetInt32("ParentID");
                    int classID = reader.GetInt32("ClassID");

                    Parent parent = await _parentService.SearchParent(parentID);
                    SchoolClass schoolClass = await _schoolClassService.GetByID(classID);
                    School school = schoolClass.TheSchool;

                    Student student = new Student { ID = id, FirstName = firstName, Surname = surName, TheParent = parent, TheSchool = school, TheSchoolClass = schoolClass };
                    students.Add(student);
                }
                await reader.CloseAsync();
            }
            return students;
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
                    int classID = reader.GetInt32("ClassID");

                    Parent parent = await _parentService.SearchParent(parentID);
                    SchoolClass schoolClass = await _schoolClassService.GetByID(classID);
                    School school = schoolClass.TheSchool;

                    Student student = new Student { ID = id, FirstName = firstName, Surname = surName, TheParent = parent, TheSchool = school, TheSchoolClass = schoolClass };
                    students.Add(student);
                }
                await reader.CloseAsync();
            }
            return students;
        }

        public async Task<List<Student>> GetByClass(int classID)
        {
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select * from student where ClassID = @ClassID", connection);
                command.Parameters.AddWithValue("@ClassID", classID);

                await command.Connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while(await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    string firstName = reader.GetString("FirstName");
                    string surName = reader.GetString("Surname");
                    int parentID = reader.GetInt32("ParentID");

                    Parent parent = await _parentService.SearchParent(parentID);
                    SchoolClass schoolClass = await _schoolClassService.GetByID(classID);
                    School school = schoolClass.TheSchool;

                    Student student = new Student { ID = id, FirstName = firstName, Surname = surName, TheParent = parent, TheSchool = school, TheSchoolClass = schoolClass };
                    students.Add(student);
                }
                await reader.CloseAsync();
            }
            return students;
        }

        public async Task<Student> GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select * from Student where ID = @ID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string firstName = reader.GetString("FirstName");
                    string surName = reader.GetString("Surname");
                    int parentID = reader.GetInt32("ParentID");
                    int classID = reader.GetInt32("ClassID");

                    Parent parent = await _parentService.SearchParent(parentID);
                    SchoolClass schoolClass = await _schoolClassService.GetByID(classID);
                    School school = schoolClass.TheSchool;

                    Student student = new Student { ID = id, FirstName = firstName, Surname = surName, TheParent = parent, TheSchool = school, TheSchoolClass = schoolClass };
                    await reader.CloseAsync();
                    return student;
                }
                return null;
            }
        }

        public async Task Update(Student student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("update student set FirstName = @FirstName, Surname = @Surname, ClassID = @ClassID where ID = @ID", connection);

                command.Parameters.AddWithValue("@ID", student.ID);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@Surname", student.Surname);
                command.Parameters.AddWithValue("@ClassID", student.TheSchoolClass.ID);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
