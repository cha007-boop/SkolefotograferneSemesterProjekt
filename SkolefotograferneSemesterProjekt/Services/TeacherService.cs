using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;
using System.Text;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class TeacherService : Connection, ITeacherService
    {
        private IUserService _userService = new UserService();
        private ISchoolService _schoolService = new SchoolService();
        public async Task<int> Add(Teacher teacher)
        {
            int userID = await _userService.Add(teacher); ;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Teacher 
                    VALUES (@ID, @FirstName, @Surname, @PhoneNumber, @SchoolID)", connection);

                cmd.Parameters.AddWithValue("@ID", userID);
                cmd.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                cmd.Parameters.AddWithValue("@Surname", teacher.Surname);
                cmd.Parameters.AddWithValue("@PhoneNumber", teacher.PhoneNumber);
                cmd.Parameters.AddWithValue("@SchoolID", teacher.TheSchool.ID);

                await cmd.ExecuteNonQueryAsync();
            }
            return userID;
        }
        public async Task<List<Teacher>> GetAll()
        {
            List<Teacher> teacherLst = [];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT Teacher.ID AS ID, FirstName, Surname, Email, PhoneNumber, SchoolID
                    FROM Teacher
                    INNER JOIN Users 
                    ON Teacher.ID = Users.ID", connection);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Teacher t = new Teacher();
                    t.ID = reader.GetInt32("ID");
                    t.FirstName = reader.GetString("FirstName");
                    t.Surname = reader.GetString("Surname");
                    t.Email = reader.GetString("Email");
                    t.PhoneNumber = reader.GetString("PhoneNumber");
                    int schoolID = reader.GetInt32("SchoolID");
                    t.TheSchool = await _schoolService.GetById(schoolID);
                    teacherLst.Add(t);
                }
            }
            return teacherLst;
        }
        public async Task Update(Teacher teacher)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmdUsers = new SqlCommand(@"
                    UPDATE Users
                    SET Email = @Email
                    WHERE ID = @ID", connection);
                cmdUsers.Parameters.AddWithValue("@ID", teacher.ID);
                cmdUsers.Parameters.AddWithValue("@Email", teacher.Email);

                SqlCommand cmdTeachers = new SqlCommand(@"
                    UPDATE Teacher
                    SET FirstName = @FirstName, Surname = @Surname, PhoneNumber = @PhoneNumber
                    WHERE ID = @ID", connection);
                cmdTeachers.Parameters.AddWithValue("@ID", teacher.ID);
                cmdTeachers.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                cmdTeachers.Parameters.AddWithValue("@Surname", teacher.Surname);
                cmdTeachers.Parameters.AddWithValue("@PhoneNumber", teacher.PhoneNumber);

                await cmdUsers.ExecuteNonQueryAsync();
                await cmdTeachers.ExecuteNonQueryAsync();
            }
        }
        public async Task Delete(Teacher teacher)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await _userService.Delete(teacher.ID);

                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM Teacher 
                    WHERE ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", teacher.ID);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<Teacher?> GetByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT Teacher.ID AS ID, FirstName, Surname, Email, PhoneNumber, SchoolID
                    FROM Teacher
                    INNER JOIN Users
                    ON Teacher.ID = Users.ID
                    WHERE Teacher.ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    Teacher t = new Teacher();
                    t.ID = reader.GetInt32("ID");
                    t.FirstName = reader.GetString("FirstName");
                    t.Surname = reader.GetString("Surname");
                    t.Email = reader.GetString("Email");
                    t.PhoneNumber = reader.GetString("PhoneNumber");
                    int schoolID = reader.GetInt32("SchoolID");
                    t.TheSchool = await _schoolService.GetById(schoolID);
                    return t;
                }
            }
            return null;
        }
        public async Task<List<Teacher>> GetBySchoolID(int id)
        {
            List<Teacher> teacherLst = [];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        t.ID, t.FirstName, t.Surname, t.PhoneNumber,
                        u.Email,
                        s.ID AS SchoolID, s.Name, s.Street, s.ZipCode, s.Country
                    FROM Teacher t
                    INNER JOIN Users u ON t.ID = u.ID
                    INNER JOIN School s ON t.SchoolID = s.ID
                    WHERE @SchoolID = s.ID", connection);
                cmd.Parameters.AddWithValue("@SchoolID", id);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Teacher t = new Teacher();
                    t.ID = reader.GetInt32("ID");
                    t.FirstName = reader.GetString("FirstName");
                    t.Surname = reader.GetString("Surname");
                    t.Email = reader.GetString("Email");
                    t.PhoneNumber = reader.GetString("PhoneNumber");
                    t.TheSchool = new School() { ID = reader.GetInt32("SchoolID"), Name = reader.GetString("Name"), Street = reader.GetString("Street"), ZipCode = reader.GetString("ZipCode"), Country = reader.GetString("Country") };
                    teacherLst.Add(t);
                }
            }
            return teacherLst;
        }
    }
}
