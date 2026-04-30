using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SysAdminService : Connection, ISysAdminService
    {
        #region Instance fields
        private IUserService userService;
        #endregion
        #region Constructor
        public SysAdminService()
        {
            userService = new UserService();
        }
        #endregion
        #region Methods
        public async Task Add(SysAdmin sysAdmin)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await userService.Add(connection, sysAdmin);
            }
        }
        public Task<List<SysAdmin>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SysAdmin> SearchByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(SysAdmin newSysAdmin)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
