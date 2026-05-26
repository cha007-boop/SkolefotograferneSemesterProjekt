using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Photo
    {
        [Display(Name ="Filnavn")]
        public string Filename { get; set; }
        [Display(Name = "Fotoevent")]
        public PhotoEvent ThePhotoEvent { get; set; }
        [Display(Name = "Skoleklasse")]
        public SchoolClass? TheSchoolClass { get; set; }
        [Display(Name = "Barn")]
        public Student? Child { get; set; }
        [Display(Name = "Uploadet dato")]
        public DateTime UploadedAt { get; set; }
        public Photo()
        {
            
        }
    }
}
