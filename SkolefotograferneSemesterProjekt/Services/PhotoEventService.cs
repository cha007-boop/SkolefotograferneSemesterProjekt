using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Pages;
using System.Data;
using System.Reflection;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoEventService : IPhotoEventService
    {
        private IPhotographerService _photographerService = new PhotographerService();
        private ISchoolAdminService _schoolAdminService = new SchoolAdminService();

        private string _insertPhotoEventString = "insert into PhotoEvent Values(@StartTime,@EndTime,@PhotographerID,@SchoolAdminID, @Location)";
        private string _selectPhotoEventString = "select * from PhotoEvent";
        private string _selectPhotoEventStringBySpecificPhotographID = "select * from PhotoEvent where PhotographerID = @PhotographerID";

        public PhotoEventService()
        {
            
        }
        public async Task<int> Add(PhotoEvent photoEvent)
        {
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                
                SqlCommand sql = new SqlCommand(_insertPhotoEventString, connection);
                await connection.OpenAsync();
                sql.Parameters.AddWithValue("@StartTime", photoEvent.StartTime);
                sql.Parameters.AddWithValue("@EndTime", photoEvent.EndTime);
                sql.Parameters.AddWithValue("@PhotographerID", photoEvent.ThePhotographer.ID);
                sql.Parameters.AddWithValue("@SchoolAdminID", photoEvent.TheSchoolAdmin.ID);
                sql.Parameters.AddWithValue("@Location", photoEvent.Location);
                var result = await sql.ExecuteNonQueryAsync();
                return Convert.ToInt32(result);
            }
        }

        public async Task<List<PhotoEvent>> ShowActivePhotoEvent() //GetAll() method
        {
            List<PhotoEvent> photoEvents = new List<PhotoEvent>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                SqlCommand sql = new SqlCommand(_selectPhotoEventString, connection);
                await sql.Connection.OpenAsync();
                SqlDataReader sqlDataReader = await sql.ExecuteReaderAsync();
                while (sqlDataReader.Read())
                {
                    int photoEventID = sqlDataReader.GetInt32("ID");
                    DateTime startTime = sqlDataReader.GetDateTime("StartTime");
                    DateTime endTime = sqlDataReader.GetDateTime("EndTime");
                    int photographerID = sqlDataReader.GetInt32("PhotographerID");
                    int schoolAdminID = sqlDataReader.GetInt32("SchoolAdminID");
                    string location = sqlDataReader.GetString("Location");

                    Photographer photographer = await _photographerService.SearchByID(photographerID);
                    SchoolAdmin schoolAdmin = await _schoolAdminService.GetById(schoolAdminID);

                    PhotoEvent photoEvent = new PhotoEvent(photoEventID, startTime, endTime, photographer, schoolAdmin, 
                    location);
                    photoEvents.Add(photoEvent);
                }
                sqlDataReader.Close();
            }
            return photoEvents;
        }
        
        public async Task<IEnumerable<PhotoEvent>> SearchEventByPhortographerID(int ID)
        {
            List<PhotoEvent> eventGetter = new List<PhotoEvent>();
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                SqlCommand sql = new SqlCommand(_selectPhotoEventStringBySpecificPhotographID, connection);
                await sql.Connection.OpenAsync();
                sql.Parameters.AddWithValue("@PhotographerID",ID);
                SqlDataReader sqlDataReader = await sql.ExecuteReaderAsync();
                while (sqlDataReader.Read())
                {
                    int photoEventID = sqlDataReader.GetInt32("ID");
                    DateTime startTime = sqlDataReader.GetDateTime("StartTime");
                    DateTime endTime = sqlDataReader.GetDateTime("EndTime");
                    int schoolAdminID = sqlDataReader.GetInt32("SchoolAdminID");
                    string location = sqlDataReader.GetString("Location");

                    Photographer photographer = await _photographerService.SearchByID(ID);
                    SchoolAdmin schoolAdmin = await _schoolAdminService.GetById(schoolAdminID);

                    PhotoEvent photoEvent = new PhotoEvent(photoEventID, startTime, endTime, photographer, schoolAdmin,
                    location);
                    eventGetter.Add(photoEvent);
                }
                sqlDataReader.Close();
            }
            return eventGetter;
        }
    }
}
