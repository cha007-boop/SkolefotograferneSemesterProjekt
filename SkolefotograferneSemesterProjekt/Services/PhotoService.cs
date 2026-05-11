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
            { "Photo.PhotoEventID", "Photo Event ID" },
            { "School.Name", "School Name" },
            { "SchoolClass.Grade, SchoolClass.Letter", "Class" },
            { "Student.FirstName", "Child first name" },
            { "Student.Surname", "Child surname" },
            { "Photo.UploadedAt", "Uploaded At" }
        };

        public Dictionary<string, string> FilterableColumns { get; } = new Dictionary<string, string>
        {
            { "Photo.Filename", "Filename" },
            { "School.Name", "School name" },
            { "Photo.SchoolID", "School ID" },
            { "Student.FirstName", "Child first name" },
            { "Student.Surname", "Child surname" },
            { "Photo.PhotoEventID", "Photo Event ID" },
            { "Photo.ClassID", "Class ID" },
            { "Photo.ChildID", "Child ID" },
            { "Student.ParentID", "Parent ID" },
            { "Parent.FirstName", "Parent first name" },
            { "Parent.Surname", "Parent surname" }

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
                    await conn.OpenAsync();
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

        public async Task<Photo> GetByFilename(string filename)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Photo WHERE Filename = @Filename";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Filename", filename);
                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
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
                    await conn.OpenAsync();
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

        public async Task<List<Photo>> GetPortraitsByStudentId(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Photo WHERE ChildID = @StudentId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    await conn.OpenAsync();
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
                    await conn.OpenAsync();
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

        public async Task<List<Photo>> Search(string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            List<Photo> photos = new List<Photo>();
            string query = @"SELECT * FROM Photo
                             JOIN PhotoEvent ON Photo.PhotoEventID = PhotoEvent.ID
                             JOIN Photographer on PhotoEvent.PhotographerID = Photographer.ID
                             LEFT OUTER JOIN Student ON Photo.ChildID = Student.ID
                             LEFT OUTER JOIN Parent ON Student.ParentID = Parent.ID
                             JOIN SchoolClass ON Photo.ClassID = SchoolClass.ID
                             JOIN Teacher ON SchoolClass.TeacherID = Teacher.ID
                             JOIN School ON Teacher.SchoolID = School.ID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                IEnumerable<string> validColumns = SortableColumns.Keys.Concat(FilterableColumns.Keys);
            }


            return photos;
        }

        public Task RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        private async Task<Photo> PhotoReader(SqlDataReader reader)
        {
            return new Photo
            {
                Filename = reader.GetString("Filename"),
                ThePhotoEvent = await _photoEventService.GetByID(reader.GetInt32("PhotoEventID")),
                TheSchoolClass = reader["ClassID"] != DBNull.Value ? await _schoolClassService.GetByID(reader.GetInt32("ClassID")) : null,
                Child = reader["ChildID"] != DBNull.Value ? await _studentService.GetById(reader.GetInt32("ChildID")) : null,
                UploadedAt = reader.GetDateTime("UploadedAt")
            };
        }
    }
}
