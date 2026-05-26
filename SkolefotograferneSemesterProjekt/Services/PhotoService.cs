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
            { "Photo.PhotoEventID", "Fotoevent ID" },
            { "School.Name", "Skole navn" },
            { "Student.FirstName", "Barnets fornavn" },
            { "Student.Surname", "Barnets efternavn" },
            { "Photo.UploadedAt", "Uploadet den" }
        };

        public Dictionary<string, string> FilterableColumns { get; } = new Dictionary<string, string>
        {
            { "Filename", "Filnavn" },
            { "Photographer.FirstName", "Fotograf fornavn" },
            { "Photographer.Surname", "Fotograf efternavn" },
            { "Photographer.ID", "Fotograf ID" },
            { "School.Name", "Skole navn" },
            { "SchoolID", "Skole ID" },
            { "Student.FirstName", "Barnets fornavn" },
            { "Student.Surname", "Barnets efternavn" },
            { "PhotoEventID", "Fotoevent ID" },
            { "Student.ClassID", "Klasse ID" },
            { "ChildID", "Barn ID" },
            { "Student.ParentID", "Forælder ID" },
            { "Parent.FirstName", "Forælder fornavn" },
            { "Parent.Surname", "Forælder efternavn" }

        };

        public async Task<string> Add(Photo photo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "INSERT INTO Photo (Filename, PhotoEventID, ClassID, ChildID, UploadedAt) VALUES (@Filename, @PhotoEventID, @ClassID, @ChildID, @UploadedAt)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filename", photo.Filename);
                    command.Parameters.AddWithValue("@PhotoEventID", photo.ThePhotoEvent?.ID ?? (object)DBNull.Value);
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
            return photo.Filename;
        }

        public async Task<List<Photo>> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
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
                    string query = "SELECT * FROM Photo WHERE ClassID = @SchoolClassId AND ChildID IS NULL";
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

        public async Task<List<Photo>> Search(string filterColumn, string filterValue, string sortColumn, string sortOrder, List<string> conditions = null)
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
                if (string.IsNullOrWhiteSpace(sortColumn))
                {
                    sortColumn = "Photo.UploadedAt";
                }
                if (string.IsNullOrWhiteSpace(filterColumn))
                {
                    filterColumn = "All";
                }

                if (!FilterableColumns.ContainsKey(filterColumn) && filterColumn != "All")
                {
                    throw new ArgumentException("Invalid column name");
                }
                if (!SortableColumns.ContainsKey(sortColumn))
                {
                    throw new ArgumentException("Invalid column name");
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(filterValue))
                    {
                        query += " WHERE ";

                        if (filterColumn == "All")
                        {
                            query += "(" + string.Join(" OR ", FilterableColumns.Keys.Select(col => $"{col} LIKE @FilterValue")) + ")";
                        }
                        else
                        {
                            query += $" {filterColumn} LIKE @FilterValue";
                        }
                        if (conditions != null && conditions.Count > 0)
                        {
                            query += " AND " + string.Join(" AND ", conditions);
                        }
                    }
                    else if (conditions != null && conditions.Count > 0)
                    {
                        query += " WHERE " + string.Join(" AND ", conditions);
                        
                    }

                    query += $" ORDER BY {sortColumn} {sortOrder}";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FilterValue", $"%{filterValue}%");

                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Photo photo = await PhotoReader(reader);
                        photos.Add(photo);
                    }
                    reader.Close();
                }
                catch
                {
                    throw;
                }
            }
            return photos;
        }

        public async Task RemovePhoto(string filename)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "DELETE FROM Photo WHERE Filename = @Filename";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filename", filename);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        private async Task<Photo> PhotoReader(SqlDataReader reader)
        {
            return new Photo
            {
                Filename = reader.GetString("Filename"),
                ThePhotoEvent = reader["PhotoEventID"] != DBNull.Value ? await _photoEventService.GetByID(reader.GetInt32("PhotoEventID")) : null,
                TheSchoolClass = reader["ClassID"] != DBNull.Value ? await _schoolClassService.GetByID(reader.GetInt32("ClassID")) : null,
                Child = reader["ChildID"] != DBNull.Value ? await _studentService.GetById(reader.GetInt32("ChildID")) : null,
                UploadedAt = reader.GetDateTime("UploadedAt")
            };
        }
    }
}
