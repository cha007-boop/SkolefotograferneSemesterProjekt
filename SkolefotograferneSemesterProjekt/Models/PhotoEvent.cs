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
        [Required(ErrorMessage = "StartTime is required")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "EndTime is required")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "PhotographerID is required")]
        public int PhotographerID { get; set; }
        [Required(ErrorMessage = "SchoolAdminID is required")]
        public int SchoolAdminID { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
        public PhotoEvent()
        {
        }

        
    }
}
