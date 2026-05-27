namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUploadIFormFile
    {
        /// <summary>
        /// Method for uploading a file 
        /// </summary>
        /// <param name="file">The file to be uploaded</param>
        /// <returns>A task representing the asynchronous operation, containing the filename of the uploaded file</returns>
        Task<string> UploadFile(IFormFile file);
    }
}
