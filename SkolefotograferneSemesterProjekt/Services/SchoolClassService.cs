using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolClassService : Connection, ISchoolClassService
    {
        private ISchoolService _schoolService = new SchoolService();
        private ITeacherService _teacherService = new TeacherService();
        private IClassBookingService _classBookingService = new ClassBookingService();

        public async Task Add(SchoolClass @class)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if(@class.Grade > 10)
                    {
                        throw new Exception();
                    }
                    await connection.OpenAsync();

                    SqlCommand sqlCommand = new SqlCommand(@"insert into SchoolClass (TeacherID, Grade, Letter, SchoolYear) values (@TeacherID, @Grade, @Letter, @SchoolYear)", connection);

                    sqlCommand.Parameters.AddWithValue("@TeacherID", @class.TheTeacher.ID);
                    sqlCommand.Parameters.AddWithValue("@Grade", @class.Grade);
                    sqlCommand.Parameters.AddWithValue("@Letter", @class.Letter);
                    sqlCommand.Parameters.AddWithValue("@SchoolYear", @class.SchoolYear);

                    await sqlCommand.ExecuteNonQueryAsync();
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
                SqlCommand command = new SqlCommand("delete from SchoolClass where ID = @ID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@ID", id);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<SchoolClass>> GetAll()
        {
            List<SchoolClass> classes = new List<SchoolClass>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select * from SchoolClass", connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    int teacherID = reader.GetInt32("TeacherID");
                    int grade = reader.GetInt32("Grade");
                    string letter = reader.GetString("Letter");
                    string year = reader.GetString("SchoolYear");


                    Teacher teacher = await _teacherService.GetByID(teacherID);
                    School school = teacher.TheSchool;

                    SchoolClass schoolClass = new SchoolClass { ID = id, TheSchool = school, TheTeacher = teacher, Grade = grade, Letter = letter, SchoolYear = year };
                    classes.Add(schoolClass);
                }
                await reader.CloseAsync();
            }
            return classes;
        }

        public async Task<List<SchoolClass>> GetAllByTeacher(int teacherid)
        {
            List<SchoolClass> classes = new List<SchoolClass>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select * from SchoolClass where TeacherID = @TeacherID", connection);
                await command.Connection.OpenAsync();

                command.Parameters.AddWithValue("@TeacherID", teacherid);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    int grade = reader.GetInt32("Grade");
                    string letter = reader.GetString("Letter");
                    string year = reader.GetString("SchoolYear");

                    Teacher teacher = await _teacherService.GetByID(teacherid);
                    School school = teacher.TheSchool;

                    SchoolClass schoolClass = new SchoolClass { ID = id, TheSchool = school, TheTeacher = teacher, Grade = grade, Letter = letter, SchoolYear = year };
                    classes.Add(schoolClass);
                }
                await reader.CloseAsync();
            }
            return classes;
        }

        public async Task<List<SchoolClass>> GetByPhotoEvent(int photoEventId)
        {
            List<SchoolClass> classes = new List<SchoolClass>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"select sc.* from SchoolClass sc join ClassBooking cb on sc.ID = cb.ClassID where cb.PhotoEventID = @PhotoEventID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@PhotoEventID", photoEventId);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    SchoolClass schoolClass = await SchoolClassReader(reader);
                    classes.Add(schoolClass);
                }
                await reader.CloseAsync();
            }
            return classes;
        }

        public async Task<SchoolClass> GetByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select * from SchoolClass where ID = @ID", connection);
                await command.Connection.OpenAsync();

                command.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    int teacherID = reader.GetInt32("TeacherID");
                    int grade = reader.GetInt32("Grade");
                    string letter = reader.GetString("Letter");
                    string year = reader.GetString("SchoolYear");

                    Teacher teacher = await _teacherService.GetByID(teacherID);
                    School school = teacher.TheSchool;

                    SchoolClass schoolClass = new SchoolClass { ID = id, TheSchool = school, TheTeacher = teacher, Grade = grade, Letter = letter, SchoolYear = year };
                    await reader.CloseAsync();
                    return schoolClass;
                }
                return null;
            }
        }

        public async Task<List<SchoolClass>> GetBySchool(int schoolID)
        {
            List<SchoolClass> classes = new List<SchoolClass>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select sc.* from SchoolClass sc join Teacher t on sc.TeacherID = t.ID where SchoolID = @SchoolID", connection);
                await command.Connection.OpenAsync();
                command.Parameters.AddWithValue("@SchoolID", schoolID);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    SchoolClass schoolClass = await SchoolClassReader(reader);
                    classes.Add(schoolClass);
                }
                await reader.CloseAsync();
            }
            return classes;
        }

        public async Task<SchoolClass> SearchSchoolClass(int schoolID, int grade, string letter, string year)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select sc.* from SchoolClass sc join Teacher t on sc.TeacherID = t.ID where t.SchoolID = @SchoolID and sc.Grade = @Grade and sc.Letter = @Letter and sc.SchoolYear = @SchoolYear", connection);
                await command.Connection.OpenAsync();

                command.Parameters.AddWithValue("@SchoolID", schoolID);
                command.Parameters.AddWithValue("@Grade", grade);
                command.Parameters.AddWithValue("@Letter", letter);
                command.Parameters.AddWithValue("@SchoolYear", year);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                if(await reader.ReadAsync())
                {
                    int id = reader.GetInt32("ID");
                    int teacherID = reader.GetInt32("TeacherID");

                    Teacher teacher = await _teacherService.GetByID(teacherID);
                    School school = teacher.TheSchool;

                    SchoolClass schoolClass = new SchoolClass { ID = id, TheSchool = school, TheTeacher = teacher, Grade = grade, Letter = letter, SchoolYear = year };
                    await reader.CloseAsync();
                    return schoolClass;
                }
                return null;
            }
        }

        public async Task Update(SchoolClass newSchoolClass)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    if (newSchoolClass.Grade > 10)
                    {
                        throw new Exception();
                    }
                    SqlCommand command = new SqlCommand("update SchoolClass set Grade = @Grade, Letter = @Letter, SchoolYear = @SchoolYear where ID = @ID", connection);
                    await command.Connection.OpenAsync();

                    command.Parameters.AddWithValue("@ID", newSchoolClass.ID);
                    command.Parameters.AddWithValue("@Grade", newSchoolClass.Grade);
                    command.Parameters.AddWithValue("@Letter", newSchoolClass.Letter);
                    command.Parameters.AddWithValue("@SchoolYear", newSchoolClass.SchoolYear);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    throw;
                }
            }
        }

        private async Task<SchoolClass> SchoolClassReader(SqlDataReader reader)
        {
            int id = reader.GetInt32("ID");
            int teacherID = reader.GetInt32("TeacherID");
            int grade = reader.GetInt32("Grade");
            string letter = reader.GetString("Letter");
            string year = reader.GetString("SchoolYear");
            Teacher teacher = await _teacherService.GetByID(teacherID);
            School school = teacher.TheSchool;
            SchoolClass schoolClass = new SchoolClass { ID = id, TheSchool = school, TheTeacher = teacher, Grade = grade, Letter = letter, SchoolYear = year };
            return schoolClass;
        }
    }
}