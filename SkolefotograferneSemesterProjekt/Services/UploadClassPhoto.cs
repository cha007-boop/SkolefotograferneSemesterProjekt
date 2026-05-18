using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class UploadClassPhoto : IUploadIFormFile
    {
        private IWebHostEnvironment _webHostEnvironment;
        private SchoolClass _schoolClass;
        public UploadClassPhoto(IWebHostEnvironment webHostEnvironment, SchoolClass schoolClass)
        {
            _webHostEnvironment = webHostEnvironment;
            _schoolClass = schoolClass;
        }

        public string UploadFile(IFormFile file)
        {
            string uniqueFilename = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/ClassPhotos");
                uniqueFilename = _schoolClass.TheSchool.ID + "_" +
                    _schoolClass.ID + "_" +
                    Guid.NewGuid().ToString() + "_" + file.FileName;
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
