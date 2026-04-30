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
        public async Task<int> Add(Teacher teacher)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                int userID = await _userService.Add(connection, teacher);

                SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Teacher 
                VALUES (@ID, @FirstName, @Surname, @PhoneNumber, @SchoolID)", connection);

                cmd.Parameters.AddWithValue("@ID", userID);
                cmd.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                cmd.Parameters.AddWithValue("@Surname", teacher.Surname);
                cmd.Parameters.AddWithValue("@PhoneNumber", teacher.PhoneNumber);
                cmd.Parameters.AddWithValue("@SchoolID", teacher.SchoolID);

                return await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<Teacher>> GetAll()
        {
            List<Teacher> teacherLst = [];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT Teacher.ID AS ID, FirstName, Surname, Email, PhoneNumber, SchoolID
                    FROM Teacher
                    INNER JOIN Users 
                    ON Teacher.ID = Users.ID;", connection);
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Teacher t = new Teacher();
                    t.ID = reader.GetInt32("ID");
                    t.FirstName = reader.GetString("FirstName");
                    t.Surname = reader.GetString("Surname");
                    t.Email = reader.GetString("Email");
                    t.PhoneNumber = reader.GetString("PhoneNumber");
                    t.SchoolID = reader.GetInt32("SchoolID");
                    teacherLst.Add(t);
                }
            }
            return teacherLst;
        }

        // Work in progress...
        public async Task Delete(Teacher teacher)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                DELETE FROM Teacher 
                WHERE ID = @ID", connection);
                await connection.OpenAsync();
                cmd.Parameters.AddWithValue("@ID", teacher.ID);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public Task Update(Teacher teacher)
        {
            throw new NotImplementedException();
        }
    }
}
