using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class UploadStudentPortrait : IUploadIFormFile
    {
        private IWebHostEnvironment _webHostEnvironment;
        private Student _student;

        public UploadStudentPortrait(IWebHostEnvironment webHostEnvironment, Student student)
        {
            _webHostEnvironment = webHostEnvironment;
            _student = student;
        }

        public string UploadFile(IFormFile file)
        {
            string uniqueFilename = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Portraits");
                uniqueFilename = _student.TheSchool.ID + "_" +
                    _student.TheSchoolClass.ID + "_" +
                    _student.ID + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFilename;
        }

    }
}
