namespace SkolefotograferneSemesterProjekt.Models
{
    public class Parent : User
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        
        public Parent()
            : base()
        {
            Role = UserRole.Parent;
        }

        public override string ToString()
        {
            return $"{FirstName} {Surname} {PhoneNumber}";
        }
    }
}
