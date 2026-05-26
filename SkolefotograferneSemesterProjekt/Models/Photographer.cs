using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Photographer : User
    {
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternavn")]
        public string Surname { get; set; }
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Website")]
        public string? Website { get; set; }
        [Display(Name = "CVR")]
        public string? CVR { get; set; }
        [Display(Name = "Facebook")]
        public string? Facebook { get; set; }
        [Display(Name = "Instagram")]
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
