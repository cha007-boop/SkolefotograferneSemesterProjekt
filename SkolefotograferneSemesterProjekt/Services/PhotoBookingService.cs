using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoBookingService : Connection, IPhotoBookingService
    {
        private IPhotoEventService _photoES = new PhotoEventService();
        public async Task<int> Book(Teacher teacher)
        {
            throw new NotImplementedException();
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    await connection.OpenAsync();

            //    int photoEventID = await _photoES.Add();

            //    SqlCommand cmd = new SqlCommand(@"
            //    INSERT INTO Teacher 
            //    VALUES (@ID, @FirstName, @Surname, @PhoneNumber, @SchoolID)", connection);

            //    cmd.Parameters.AddWithValue("@ID", userID);
            //    cmd.Parameters.AddWithValue("@FirstName", teacher.FirstName);
            //    cmd.Parameters.AddWithValue("@Surname", teacher.Surname);
            //    cmd.Parameters.AddWithValue("@PhoneNumber", teacher.PhoneNumber);
            //    cmd.Parameters.AddWithValue("@SchoolID", teacher.SchoolID);

            //    return await cmd.ExecuteNonQueryAsync();
            //}
        }
        public Task<ClassBooking> GetByID(int id)
        {
            throw new NotImplementedException();
        }
        public Task Update(ClassBooking classBooking)
        {
            throw new NotImplementedException();
        }
        public Task Delete(ClassBooking classBooking)
        {
            throw new NotImplementedException();
        }


    }
}
