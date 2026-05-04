using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class ClassBooking
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public PhotoEvent ThePhotoEvent { get; set; }
        public Teacher TheTeacher { get; set; }
        public SchoolClass TheSchoolClass { get; set; }
        public ClassBooking()
        {
            
        }
        
    }
}
