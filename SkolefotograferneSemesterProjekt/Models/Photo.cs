namespace SkolefotograferneSemesterProjekt.Models
{
    public class Photo
    {
        public string Filename { get; set; }
        public int PhotoEventID { get; set; }
        public int ClassID { get; set; }
        public int? ChildID { get; set; }
        public Photo()
        {
            
        }
    }
}
