using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Services;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class PhotoEvent
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

        
    }
}
