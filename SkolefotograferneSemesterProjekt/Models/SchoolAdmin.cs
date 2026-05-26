using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class SchoolAdmin : User
    {
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Kontaktperson")]
        public string ContactPerson { get; set; }
        [Display(Name = "Skole")]
        public School TheSchool { get; set; }
        public SchoolAdmin()
        {
            Role = UserRole.SchoolAdmin;
        }
    }
}
