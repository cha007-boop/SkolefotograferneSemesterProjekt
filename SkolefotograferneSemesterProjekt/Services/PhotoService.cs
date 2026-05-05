using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoService : IPhotoService
    {
        private IPhotoEventService _photoEventService = new PhotoEventService();
        private IStudentService _studentService = new StudentService();
        private ISchoolClassService _schoolClassService = new SchoolClassService();

        public async Task Add(Photo photo)
        {
            using (SqlConnection connection = new SqlConnection("YourConnectionStringHere"))
            {
                try
                {
                    string query = "INSERT INTO Photos (Filename, PhotoEventId, SchoolClassId, StudentId) VALUES (@Filename, @PhotoEventId, @SchoolClassId, @StudentId)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filename", photo.Filename);
                    command.Parameters.AddWithValue("@PhotoEventId", photo.ThePhotoEvent.ID);
                    command.Parameters.AddWithValue("@SchoolClassId", photo.TheSchoolClass?.ID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@StudentId", photo.Child?.ID ?? (object)DBNull.Value);
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

        public Task<List<Photo>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Photo> GetByFilename(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetClassPhotosByClass(SchoolClass schoolClass)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> GetPortraitsByStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Task RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
