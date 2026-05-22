namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUploadIFormFile
    {
        Task<string> UploadFile(IFormFile file);
    }
}
