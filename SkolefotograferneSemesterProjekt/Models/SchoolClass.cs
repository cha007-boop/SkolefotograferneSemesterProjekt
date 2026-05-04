namespace SkolefotograferneSemesterProjekt.Models
{
    public class SchoolClass
    {
        public int ID { get; set; }
        public School TheSchool { get; set; }
        public Teacher TheTeacher { get; set; }
        public int Grade { get; set; }
        public string Letter { get; set; }
        public string SchoolYear { get; set; }
        public SchoolClass()
        {
            
        }
    }
}
