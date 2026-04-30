using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoEventService : IPhotoEventService
    {
        private string _insertPhotoEventString = "insert into PhotoEvent Values(@StartTime,@EndTime,@PhotographerID,@SchoolAdminID, @Location)";
        
        
        public PhotoEventService()
        {
            
        }
        public async Task Add(PhotoEvent photoEvent)
        {
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                
                SqlCommand sql = new SqlCommand(_insertPhotoEventString, connection);
                await connection.OpenAsync();
                sql.Parameters.AddWithValue("@StartTime", photoEvent.StartTime);
                sql.Parameters.AddWithValue("@EndTIme", photoEvent.EndTime);
                sql.Parameters.AddWithValue("@PhotographerID", photoEvent.PhotographerID);
                sql.Parameters.AddWithValue("@SchoolAdminID", photoEvent.SchoolAdminID);
                sql.Parameters.AddWithValue("@Location", photoEvent.Location);
                await sql.ExecuteNonQueryAsync();
            }
        }
    }
}
