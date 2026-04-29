namespace SkolefotograferneSemesterProjekt.Models
{
    public class Parent : User
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public bool Consent { get; set; } = false;
        public Parent()
            : base()
        {
            Role = UserRole.Parent;
        }
    }
}
