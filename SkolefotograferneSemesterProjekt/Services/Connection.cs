using SkolefotograferneSemesterProjekt.Pages;

namespace SkolefotograferneSemesterProjekt.Services
{
    public abstract class Connection
    {
        protected string connectionString = Secret.ConnectionString;
    }
}
