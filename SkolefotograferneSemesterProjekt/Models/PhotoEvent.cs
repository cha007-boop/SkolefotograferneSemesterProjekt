namespace SkolefotograferneSemesterProjekt.Models
{
    public class PhotoEvent
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PhotographerID { get; set; }
        public int SchoolAdminID { get; set; }
        public PhotoEvent()
        {
            
        }
        
    }
}
