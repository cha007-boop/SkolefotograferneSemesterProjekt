using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class ClassBooking
    {
        public int ID { get; set; }
        [Display(Name = "Start tid")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Fotoevent")]
        public PhotoEvent ThePhotoEvent { get; set; }
        [Display(Name = "Lærer")]
        public Teacher TheTeacher { get; set; }
        [Display(Name = "Klasse")]
        public SchoolClass TheSchoolClass { get; set; }
        public ClassBooking()
        {
            
        }
        
    }
}
