namespace SkolefotograferneSemesterProjekt.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int ParentID { get; set; }
        public int SchoolID { get; set; }
        public int ClassID { get; set; }
        public Student()
        {
            
        }
    }
}
