namespace SkolefotograferneSemesterProjekt.Models
{
    public class SchoolClass
    {
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public int TeacherID { get; set; }
        public int Grade { get; set; }
        public string Letter { get; set; }
        public string SchoolYear { get; set; }
        public SchoolClass()
        {
            
        }
    }
}
