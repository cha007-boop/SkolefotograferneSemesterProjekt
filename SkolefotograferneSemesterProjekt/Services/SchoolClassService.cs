using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolClassService : Connection, ISchoolClassService
    {
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

                    SqlCommand sqlCommand = new SqlCommand(@"insert into SchoolClass (ID, SchoolID, TeacherID, Grade, Letter, SchoolYear) values (@ID, @SchoolID, @TeacherID, @Grade, @Letter, @SchoolYear)", connection);

                    sqlCommand.Parameters.AddWithValue("@ID", @class.ID);
                    sqlCommand.Parameters.AddWithValue("@SchoolID", @class.SchoolID);
                    sqlCommand.Parameters.AddWithValue("@TeacherID", @class.TeacherID);
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
                    int schoolID = reader.GetInt32("SchoolID");
                    int teacherID = reader.GetInt32("TeacherID");
                    int grade = reader.GetInt32("Grade");
                    string letter = reader.GetString("Letter");
                    string year = reader.GetString("SchoolYear");
                    SchoolClass schoolClass = new SchoolClass { ID = id, SchoolID = schoolID, TeacherID = teacherID, Grade = grade, Letter = letter, SchoolYear = year };
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
                    int schoolID = reader.GetInt32("SchoolID");
                    int teacherID = reader.GetInt32("TeacherID");
                    int grade = reader.GetInt32("Grade");
                    string letter = reader.GetString("Letter");
                    string year = reader.GetString("SchoolYear");
                    SchoolClass schoolClass = new SchoolClass { ID = id, SchoolID = schoolID, TeacherID = teacherID, Grade = grade, Letter = letter, SchoolYear = year };
                    await reader.CloseAsync();
                    return schoolClass;
                }
                return null;
            }
        }

        public async Task<SchoolClass> SearchSchoolClass(int schoolID, int grade, string letter, string year)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select * from SchoolClass where SchoolID = @SchoolID and Grade = @Grade and Letter = @Letter and SchoolYear = @SchoolYear", connection);
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

                    SchoolClass schoolClass = new SchoolClass { ID = id, SchoolID = schoolID, TeacherID = teacherID, Grade = grade, Letter = letter, SchoolYear = year };
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
    }
}