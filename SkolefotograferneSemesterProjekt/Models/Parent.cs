using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Parent : User
    {
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternavn")]
        public string Surname { get; set; }
        [Display(Name = "Telefonnummer")]
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
