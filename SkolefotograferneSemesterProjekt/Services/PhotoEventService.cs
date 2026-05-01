using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;
using System.Reflection;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoEventService : IPhotoEventService
    {
        private string _insertPhotoEventString = "insert into PhotoEvent Values(@StartTime,@EndTime,@PhotographerID,@SchoolAdminID, @Location)";
        private string _selectPhotoEventString = "select * from PhotoEvent"; 
        
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

        public async Task<List<PhotoEvent>> ShowYourActivePhotoEvent()
        {
            List<PhotoEvent> yourActivePhotoEvents = new List<PhotoEvent>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                SqlCommand sql = new SqlCommand(_selectPhotoEventString, connection);
                await sql.Connection.OpenAsync();
                SqlDataReader sqlDataReader = await sql.ExecuteReaderAsync();
                while (sqlDataReader.Read())
                {
                    int photoEventID = sqlDataReader.GetInt32("PhotoEventID");
                    DateTime startTime = sqlDataReader.GetDateTime("StartTime");
                    DateTime endTime = sqlDataReader.GetDateTime("EndTime");
                    int photographerID = sqlDataReader.GetInt32("PhotographerID");
                    int schoolAdminID = sqlDataReader.GetInt32("SchoolAdminID");
                    string location = sqlDataReader.GetString("Location");
                    PhotoEvent photoEvent = new PhotoEvent(photoEventID, startTime, endTime, photographerID, schoolAdminID, location);
                    yourActivePhotoEvents.Add(photoEvent);
                    //if (photographerID == (HttpContextAccessor.HttpContext.Session.GetInt32("UserRole") == 1))
                    //{
                    //    yourActivePhotoEvents.Add(photoEvent);
                    //}
                }
                sqlDataReader.Close();
            }
            return yourActivePhotoEvents;
        }
    }
}
