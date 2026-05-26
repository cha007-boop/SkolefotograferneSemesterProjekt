using Amazon.Runtime.SharedInterfaces;
using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Teacher : User
    {
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternavn")]
        public string Surname { get; set; }
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Skole")]
        public School TheSchool { get; set; }
        public Teacher()
            :base()
        {
            Role = UserRole.Teacher;
        }
        
    }
}
