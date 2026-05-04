using System.Threading.Tasks;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Services;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Parent TheParent { get; set; }
        public School TheSchool { get; set; }
        public SchoolClass TheSchoolClass { get; set; }
        public Student()
        {

        }
    }
}
