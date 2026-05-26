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
        [Display(Name = "Starttid")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Sluttid")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Fotograf")]
        public Photographer ThePhotographer { get; set; }
        [Display(Name = "Skoleadministrator")]
        public SchoolAdmin TheSchoolAdmin { get; set; }
        [Display(Name = "Lokation")]
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
