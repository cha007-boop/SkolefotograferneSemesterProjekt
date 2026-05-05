using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class ClassBookingService : Connection, IClassBookingService
    {
        private IPhotoEventService _photoES = new PhotoEventService();
        public async Task<int> Book(ClassBooking cs, SchoolClass sc, int photoEventID)
        {
            PhotoEvent? photoEvent = await _photoES.GetByID(photoEventID);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                INSERT INTO ClassBooking 
                VALUES (@ID, @StartTime, @PhotoEventID, @TeacherID, @ClassID)", connection);

                cmd.Parameters.AddWithValue("@ID", cs.ID);
                cmd.Parameters.AddWithValue("@StartTime", cs.StartTime);
                cmd.Parameters.AddWithValue("@PhotoEventID", cs.ThePhotoEvent.ID);
                cmd.Parameters.AddWithValue("@TeacherID", cs.TheTeacher.ID);
                cmd.Parameters.AddWithValue("@ClassID", sc.ID);

                return await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<ClassBooking>> GetAll()
        {
            List<ClassBooking> cbList = [];
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT ID, StartTime, PhotoEventID, TeacherID, ClassID
                    FROM ClassBooking", connection);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    ClassBooking cb = new ClassBooking();
                    cb.ID = reader.GetInt32("ID");
                    cb.StartTime = reader.GetDateTime("StartTime");
                    cb.ThePhotoEvent = new PhotoEvent { ID = reader.GetInt32("PhotoEventID") };
                    cb.TheTeacher = new Teacher { ID = reader.GetInt32("TeacherID") };
                    cb.TheSchoolClass = new SchoolClass { ID = reader.GetInt32("ClassID") };
                    cbList.Add(cb);
                }
            }
            return cbList;
        }
        public async Task<ClassBooking> GetByID(int id)
        {
            List<ClassBooking> cbList = [];
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        cs.ID, cs.StartTime, 
                        p.ID AS PhotoEventID, p.StartTime AS PeStartTime, p.EndTime AS PeEndTime,
                        t.ID AS TeacherID, t.FirstName, t.Surname, t.PhoneNumber,
                        sc.ID AS ClassID, sc.Grade, sc.Letter, sc.SchoolYear
                    FROM ClassBooking cs
                    INNER JOIN PhotoEvent p ON cs.PhotoEventID = p.ID
                    INNER JOIN Teacher t ON cs.TeacherID = t.ID
                    INNER JOIN SchoolClass sc ON cs.ClassID = sc.ID
                    WHERE cs.ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    ClassBooking cb = new ClassBooking();
                    cb.ID = reader.GetInt32("ID");
                    cb.StartTime = reader.GetDateTime("StartTime");
                    cb.ThePhotoEvent = new PhotoEvent { ID = reader.GetInt32("PhotoEventID"), StartTime = reader.GetDateTime("PeStartTime"), EndTime = reader.GetDateTime("PeEndTime") };
                    cb.TheTeacher = new Teacher { ID = reader.GetInt32("TeacherID"), FirstName = reader.GetString("FirstName"), Surname = reader.GetString("Surname"), PhoneNumber = reader.GetString("PhoneNumber") };
                    cb.TheSchoolClass = new SchoolClass { ID = reader.GetInt32("ClassID"), Grade = reader.GetInt32("Grade"), Letter = reader.GetString("Letter"), SchoolYear = reader.GetString("SchoolYear") };
                    return cb;
                }
            }
            return null;
        }
        public async Task Update(ClassBooking classBooking)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    UPDATE ClassBooking
                    SET StartTime = @StartTime
                    WHERE ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", classBooking.ID);
                cmd.Parameters.AddWithValue("@StartTime", classBooking.StartTime);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task Delete(ClassBooking classBooking)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    DELETE FROM ClassBooking 
                    WHERE ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", classBooking.ID);

                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<List<ClassBooking>> GetBookingsByTeacher(Teacher teacher)
        {
            List<ClassBooking> cbList = [];
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT ID, StartTime, PhotoEventID, TeacherID, ClassID
                    FROM ClassBooking
                    WHERE TeacherID = @ID", connection);
                    cmd.Parameters.AddWithValue("@ID", teacher.ID);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    ClassBooking cb = new ClassBooking();
                    cb.ID = reader.GetInt32("ID");
                    cb.StartTime = reader.GetDateTime("StartTime");
                    cb.ThePhotoEvent = new PhotoEvent { ID = reader.GetInt32("PhotoEventID") };
                    cb.TheTeacher = new Teacher { ID = reader.GetInt32("TeacherID") };
                    cb.TheSchoolClass = new SchoolClass { ID = reader.GetInt32("ClassID") };
                    cbList.Add(cb);
                }
            }
            return cbList;
        }

        public async Task<List<ClassBooking>> GetBookingsByPhotoEvent(PhotoEvent photoEvent)
        {
            List<ClassBooking> cbList = [];
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT ID, StartTime, PhotoEventID, TeacherID, ClassID
                    FROM ClassBooking
                    WHERE PhotoEventID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", photoEvent.ID);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    ClassBooking cb = new ClassBooking();
                    cb.ID = reader.GetInt32("ID");
                    cb.StartTime = reader.GetDateTime("StartTime");
                    cb.ThePhotoEvent = new PhotoEvent { ID = reader.GetInt32("PhotoEventID") };
                    cb.TheTeacher = new Teacher { ID = reader.GetInt32("TeacherID") };
                    cb.TheSchoolClass = new SchoolClass { ID = reader.GetInt32("ClassID") };
                    cbList.Add(cb);
                }
            }
            return cbList;
        }
    }
}
