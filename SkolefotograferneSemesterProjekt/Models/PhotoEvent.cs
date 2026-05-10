using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class PhotoEvent
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Photographer ThePhotographer { get; set; }
        public SchoolAdmin TheSchoolAdmin { get; set; }
        public SchoolClass TheSchoolClass { get; set; }
        
        public string Location { get; set; }
        public PhotoEvent()
        {
            
        }
        public PhotoEvent(int id, DateTime startTime, DateTime endTime, Photographer thePhotographer, SchoolAdmin theSchoolAdmin, string location)
        {
            ID = id;
            StartTime = startTime;
            EndTime = endTime;
            ThePhotographer = thePhotographer;
            TheSchoolAdmin = theSchoolAdmin;
            Location = location;
        }
    }
}
