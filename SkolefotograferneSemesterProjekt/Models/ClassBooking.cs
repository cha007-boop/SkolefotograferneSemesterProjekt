using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class ClassBooking
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public int PhotoEventID { get; set; }
        public int TeacherID { get; set; }
        public int ClassID { get; set; }
        public ClassBooking()
        {
            
        }
        
    }
}
