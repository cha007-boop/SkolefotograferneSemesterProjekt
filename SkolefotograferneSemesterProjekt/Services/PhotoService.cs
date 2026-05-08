using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoService : Connection, IPhotoService
    {
        private IPhotoEventService _photoEventService = new PhotoEventService();
        private IStudentService _studentService = new StudentService();
        private ISchoolClassService _schoolClassService = new SchoolClassService();

        public Dictionary<string, string> SortableColumns { get; } = new Dictionary<string, string>
        {
            { "Photo.Filename", "Filename" },
            { "Photo.PhotoEventID", "Photo Event ID" },
            { "Photo.ClassID", "Class ID" },
            { "Photo.ChildID", "Child ID" },
            { "Photo.UploadedAt", "Uploaded At" }
        };

        public Dictionary<string, string> FilterableColumns { get; } = new Dictionary<string, string>
        {
            { "Photo.Filename", "Filename" },
            { "School.Name", "School name" },
            { "Photo.SchoolID", "School ID" },
            { "Child.FirstName", "Child First name" },
            { "Child.Surname", "Child Surname" },
            { "Photo.PhotoEventID", "Photo Event ID" },
            { "Photo.ClassID", "Class ID" },
            { "Photo.ChildID", "Child ID" },
            { "Student.ParentID", "Parent ID" }
        };

        public async Task Add(Photo photo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "INSERT INTO Photo (Filename, PhotoEventID, ClassID, ChildID, UploadedAt) VALUES (@Filename, @PhotoEventID, @ClassID, @ChildID, @UploadedAt)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filename", photo.Filename);
                    command.Parameters.AddWithValue("@PhotoEventID", photo.ThePhotoEvent.ID);
                    command.Parameters.AddWithValue("@ClassID", photo.TheSchoolClass?.ID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ChildID", photo.Child?.ID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UploadedAt", photo.UploadedAt);
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<List<Photo>> GetAll()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    string query = "SELECT * FROM Photo";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Photo> photos = new List<Photo>();
                    while (reader.Read())
                    {
                        Photo photo = await PhotoReader(reader);
                        photos.Add(photo);
                    }
                    return photos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<Photo> GetByFilename(string filename)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Photo WHERE Filename = @Filename";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Filename", filename);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return await PhotoReader(reader);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<List<Photo>> GetClassPhotosByClassId(int schoolClassId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Photo WHERE ClassId = @SchoolClassId AND ChildID IS NULL";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SchoolClassId", schoolClassId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Photo> photos = new List<Photo>();
                    while (reader.Read())
                    {
                        Photo photo = await PhotoReader(reader);
                        photos.Add(photo);
                    }
                    return photos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<List<Photo>> GetPortraitsByStudentId(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Photo WHERE ChildID = @StudentId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    conn.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    List<Photo> photos = new List<Photo>();
                    while (await reader.ReadAsync())
                    {
                        Photo photo = await PhotoReader(reader);
                        photos.Add(photo);
                    }
                    return photos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<List<Photo>> GetByPhotoEventId(int photoEventId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Photo WHERE PhotoEventID = @PhotoEventId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PhotoEventId", photoEventId);
                    conn.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    List<Photo> photos = new List<Photo>();
                    while (await reader.ReadAsync())
                    {
                        Photo photo = await PhotoReader(reader);
                        photos.Add(photo);
                    }
                    return photos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        //public async Task<List<Photo>> Search(string filterColumn, string filterValue, string sortColumn, string sortOrder)
        //{
        //    string query = @"SELECT * FROM Photo
        //                     JOIN PhotoEvent ON Photo.PhotoEventID = PhotoEvent.ID
        //                     JOIN Photographer on PhotoEvent.PhotographerID = Photographer.ID
        //                     LEFT OUTER JOIN Student ON Photo.ChildID = Student.ID
        //                     LEFT OUTER JOIN Parent ON Student.ParentID = Parent.ID
        //                     JOIN SchoolClass ON Photo.ClassID = SchoolClass.ID
        //                     JOIN Teacher ON SchoolClass.TeacherID = Teacher.ID
        //                     JOIN School ON Teacher.SchoolID = School.ID";

        //    List<string> conditions = new List<string>();

        //}

        public Task RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        private async Task<Photo> PhotoReader(SqlDataReader reader)
        {
            return new Photo
            {
                Filename = reader.GetString("Photo.Filename"),
                ThePhotoEvent = await _photoEventService.GetByID(reader.GetInt32("Photo.PhotoEventID")),
                TheSchoolClass = reader["Photo.ClassID"] != DBNull.Value ? await _schoolClassService.GetByID(reader.GetInt32("Photo.ClassID")) : null,
                Child = reader["Photo.ChildID"] != DBNull.Value ? await _studentService.GetById(reader.GetInt32("Photo.ChildID")) : null,
                UploadedAt = reader.GetDateTime("Photo.UploadedAt")
            };
        }
    }
}
