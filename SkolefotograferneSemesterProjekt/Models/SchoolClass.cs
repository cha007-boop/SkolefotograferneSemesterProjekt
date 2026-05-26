using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class SchoolClass
    {
        public int ID { get; set; }
        [Display(Name = "Skole")]
        public School TheSchool { get; set; }
        [Display(Name = "Lærer")]
        public Teacher TheTeacher { get; set; }
        [Display(Name = "Klassetrin")]
        public int Grade { get; set; }
        [Display(Name = "Bogstav")]
        public string Letter { get; set; }
        [Display(Name = "Skoleår")]
        public string SchoolYear { get; set; }
        public SchoolClass()
        {

        }
    }
}
