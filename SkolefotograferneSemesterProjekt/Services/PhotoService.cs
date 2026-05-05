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

        public async Task Add(Photo photo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "INSERT INTO Photo (Filename, PhotoEventID, ClassID, ChildID) VALUES (@Filename, @PhotoEventID, @ClassID, @ChildID)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filename", photo.Filename);
                    command.Parameters.AddWithValue("@PhotoEventID", photo.ThePhotoEvent.ID);
                    command.Parameters.AddWithValue("@ClassID", photo.TheSchoolClass?.ID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ChildID", photo.Child?.ID ?? (object)DBNull.Value);
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
                        Photo photo = new Photo
                        {
                            Filename = reader.GetString("Filename"),
                            ThePhotoEvent = await _photoEventService.GetByID(reader.GetInt32("PhotoEventID")),
                            TheSchoolClass = reader["ClassID"] != DBNull.Value ? await _schoolClassService.GetByID(reader.GetInt32("ClassID")) : null,
                            Child = reader["ChildID"] != DBNull.Value ? await _studentService.GetById(reader.GetInt32("ChildID")) : null
                        };
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
                        return new Photo
                        {
                            Filename = reader.GetString("Filename"),
                            ThePhotoEvent = await _photoEventService.GetByID(reader.GetInt32("PhotoEventID")),
                            TheSchoolClass = reader["ClassID"] != DBNull.Value ? await _schoolClassService.GetByID(reader.GetInt32("ClassID")) : null,
                            Child = reader["ChildID"] != DBNull.Value ? await _studentService.GetById(reader.GetInt32("ChildID")) : null
                        };
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
                    string query = "SELECT * FROM Photo WHERE ClassId = @SchoolClassId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SchoolClassId", schoolClassId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Photo> photos = new List<Photo>();
                    while (reader.Read())
                    {
                        Photo photo = new Photo
                        {
                            Filename = reader.GetString("Filename"),
                            ThePhotoEvent = await _photoEventService.GetByID(reader.GetInt32("PhotoEventID")),
                            TheSchoolClass = await _schoolClassService.GetByID(reader.GetInt32("ClassID")),
                            Child = reader["ChildID"] != DBNull.Value ? await _studentService.GetById(reader.GetInt32("ChildID")) : null
                        };
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
                        Photo photo = new Photo
                        {
                            Filename = reader.GetString("Filename"),
                            ThePhotoEvent = await _photoEventService.GetByID(reader.GetInt32("PhotoEventID")),
                            TheSchoolClass = reader["ClassID"] != DBNull.Value ? await _schoolClassService.GetByID(reader.GetInt32("ClassID")) : null,
                            Child = await _studentService.GetById(reader.GetInt32("ChildID"))
                        };
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

        public Task<List<Photo>> GetByPhotoEventId(int photoEventId)
        {
            throw new NotImplementedException();
        }

        public Task RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
