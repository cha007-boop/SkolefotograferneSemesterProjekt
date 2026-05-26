using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class School
    {
        public int ID { get; set; }
        [Display(Name = "Skole navn")]
        public string Name { get; set; }
        [Display(Name = "Antal elever")]
        public int StudentCount { get; set; }
        [Display(Name = "Vej")]
        public string Street { get; set; }
        [Display(Name = "Postnummer")]
        public string ZipCode { get; set; }
        [Display(Name = "Land")]
        public string Country { get; set; }
        public School()
        {
            
        }

        public override string ToString()
        {
            return $"{ID} {Name} {Street} {ZipCode} {Country} {StudentCount}";
        }
    }
}
