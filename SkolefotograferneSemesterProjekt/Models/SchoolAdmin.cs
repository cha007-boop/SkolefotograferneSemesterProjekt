namespace SkolefotograferneSemesterProjekt.Models
{
    public class SchoolAdmin : User
    {
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public School TheSchool { get; set; }
        public SchoolAdmin()
        {
            Role = UserRole.SchoolAdmin;
        }
    }
}
