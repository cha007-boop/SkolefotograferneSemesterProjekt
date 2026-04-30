using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Services;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class PhotoEvent:IPhotoEventService
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PhotographerID { get; set; }
        public int SchoolAdminID { get; set; }
        public string Location { get; set; }
        public PhotoEvent()
        {
        }

        public async Task Add(PhotoEvent photoEvent)
        {
            using (SqlConnection connection = new SqlConnection(Services.Secret.ConnectionString))
            {
                SqlCommand sql = new SqlCommand("insert into PhotoEvent Values(@StartDate,@EndDate,@PhotographerID,@SchoolAdminID)");
                await sql.Connection.OpenAsync();
                sql.Parameters.AddWithValue("StartDate", StartTime);
                sql.Parameters.AddWithValue("EndDate", EndTime);
                sql.Parameters.AddWithValue("PhotographerID", PhotographerID);
                sql.Parameters.AddWithValue("SchoolAdminID", SchoolAdminID);
                sql.Parameters.AddWithValue("Location", Location);
                await sql.ExecuteNonQueryAsync();
            }
        }
    }
}
