namespace SkolefotograferneSemesterProjekt.Models
{
    public class SchoolAdmin : User
    {
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public int SchoolID { get; set; }
        public SchoolAdmin()
        {
            Role = UserRole.SchoolAdmin;
        }
    }
}
