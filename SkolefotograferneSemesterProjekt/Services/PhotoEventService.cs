using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Pages;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class PhotoEventService : IPhotoEventService
    {
        private IPhotographerService _photographerService = new PhotographerService();
        private ISchoolAdminService _schoolAdminService = new SchoolAdminService();
        private ISchoolService _schoolService = new SchoolService();

        private string _insertPhotoEventString = "insert into PhotoEvent Values(@StartTime,@EndTime,@PhotographerID,@SchoolAdminID, @Location)";
        private string _selectPhotoEventString = "select * from PhotoEvent";
        private string _selectPhotoEventStringBySpecificPhotographID = "select * from PhotoEvent where PhotographerID = @PhotographerID";
        private string _deletePhotoEventString = "Delete from PhotoEvent where ID = @ID";

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
        
        public async Task<List<PhotoEvent>> SearchEventByPhortographerID(int ID)
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
        public async Task<List<PhotoEvent>> GetAll()
        {
            List<PhotoEvent> photoEventList = [];
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        pe.ID, pe.StartTime, pe.EndTime, pe.Location,
                        p.ID AS PhotographerID, p.FirstName, p.Surname, p.PhoneNumber, p.Website, p.CVR, p.Facebook, p.Instagram,
                        sa.ID AS SchoolAdminID, sa.PhoneNumber, sa.ContactPerson, sa.SchoolID
                    FROM PhotoEvent pe
                    INNER JOIN Photographer p ON pe.PhotographerID = p.ID
                    INNER JOIN SchoolAdmin sa ON pe.SchoolAdminID = sa.ID", connection);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    PhotoEvent photoEvent = new PhotoEvent();
                    photoEvent.ID = reader.GetInt32("ID");
                    photoEvent.StartTime = reader.GetDateTime("StartTime");
                    photoEvent.EndTime = reader.GetDateTime("EndTime");
                    //photoEvent.Location = reader.GetString("Location");
                    photoEvent.Location = reader.IsDBNull("Location") ? "Location is not set" : reader.GetString("Location");
                    photoEvent.ThePhotographer = new Photographer { ID = reader.GetInt32("PhotographerID"), FirstName = reader.GetString("FirstName"), Surname = reader.GetString("Surname"), PhoneNumber = reader.GetString("PhoneNumber"), Website = reader.GetString("Website"), CVR = reader.GetString("CVR"), Facebook = reader.GetString("Facebook"), Instagram = reader.GetString("Instagram") };
                    photoEvent.TheSchoolAdmin = new SchoolAdmin { ID = reader.GetInt32("SchoolAdminID"), PhoneNumber = reader.GetString("PhoneNumber"), ContactPerson = reader.GetString("ContactPerson"), TheSchool = await _schoolService.GetById(reader.GetInt32("SchoolID")) };
                    photoEventList.Add(photoEvent);
                }
            }
            return photoEventList;
        }
        public async Task<PhotoEvent?> GetByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        pe.ID, pe.StartTime, pe.EndTime, pe.Location,
                        p.ID AS PhotographerID, p.FirstName, p.Surname, p.PhoneNumber, p.Website, p.CVR, p.Facebook, p.Instagram,
                        sa.ID AS SchoolAdminID, sa.PhoneNumber, sa.ContactPerson, sa.SchoolID
                    FROM PhotoEvent pe
                    INNER JOIN Photographer p ON pe.PhotographerID = p.ID
                    INNER JOIN SchoolAdmin sa ON pe.SchoolAdminID = sa.ID
                    WHERE pe.ID = @ID", connection);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (reader.Read())
                {
                    PhotoEvent photoEvent = new PhotoEvent();
                    photoEvent.ID = reader.GetInt32("ID");
                    photoEvent.StartTime = reader.GetDateTime("StartTime");
                    photoEvent.EndTime = reader.GetDateTime("EndTime");
                    //photoEvent.Location = reader.GetString("Location");
                    photoEvent.Location = reader.IsDBNull("Location") ? "Location is not set" : reader.GetString("Location");
                    photoEvent.ThePhotographer = new Photographer { ID = reader.GetInt32("PhotographerID"), FirstName = reader.GetString("FirstName"), Surname = reader.GetString("Surname"), PhoneNumber = reader.GetString("PhoneNumber"), Website = reader.GetString("Website"), CVR = reader.GetString("CVR"), Facebook = reader.GetString("Facebook"), Instagram = reader.GetString("Instagram") };
                    photoEvent.TheSchoolAdmin = new SchoolAdmin { ID = reader.GetInt32("SchoolAdminID"), PhoneNumber = reader.GetString("PhoneNumber"), ContactPerson = reader.GetString("ContactPerson"), TheSchool = await _schoolService.GetById(reader.GetInt32("SchoolID")) };
                    return photoEvent;
                }
            }
            return null;
        }

        public async Task DeletePhotoEvent(PhotoEvent photoEvent)
        {
            using (SqlConnection connection = new SqlConnection(Secret.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(_deletePhotoEventString, connection);
                await sqlCommand.Connection.OpenAsync();
                sqlCommand.Parameters.AddWithValue("@ID", photoEvent.ID);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
