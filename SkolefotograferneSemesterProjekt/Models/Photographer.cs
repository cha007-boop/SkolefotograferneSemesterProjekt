namespace SkolefotograferneSemesterProjekt.Models
{
    public class Photographer : User
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string? Website { get; set; }
        public string? CVR { get; set; } 
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public Photographer()
            :base()
        {
            Role = UserRole.Photographer;
        }

        public string FilterAll()
        {
            return $"{FirstName ?? ""} {Surname ?? ""} {PhoneNumber ?? ""} {Email ?? ""}";
        }
    }
}
