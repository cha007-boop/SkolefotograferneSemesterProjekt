using Amazon.Runtime.SharedInterfaces;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Teacher : User
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int SchoolID { get; set; }
        public Teacher()
            :base()
        {
            Role = UserRole.Teacher;
        }
        
    }
}
