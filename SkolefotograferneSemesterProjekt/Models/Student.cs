using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternavn")]
        public string Surname { get; set; }
        [Display(Name = "Forælder")]
        public Parent TheParent { get; set; }
        [Display(Name = "Skole")]
        public School TheSchool { get; set; }
        [Display(Name = "Skoleklasse")]
        public SchoolClass TheSchoolClass { get; set; }
        public Student()
        {

        }
    }
}
