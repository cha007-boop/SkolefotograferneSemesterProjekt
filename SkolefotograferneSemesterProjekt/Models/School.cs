namespace SkolefotograferneSemesterProjekt.Models
{
    public class School
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StudentCount { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public School()
        {
            
        }
    }
}
