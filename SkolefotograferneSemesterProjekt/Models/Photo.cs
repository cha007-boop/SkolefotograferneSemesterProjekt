namespace SkolefotograferneSemesterProjekt.Models
{
    public class Photo
    {
        public string Filename { get; set; }
        public PhotoEvent ThePhotoEvent { get; set; }
        public SchoolClass? TheSchoolClass { get; set; }
        public Student? Child { get; set; }
        public Photo()
        {
            
        }
    }
}
