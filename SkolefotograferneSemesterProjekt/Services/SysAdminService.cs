using System.Data;
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
        public async Task<List<SysAdmin>> GetAll()
        {
            List<SysAdmin> sysAdmins = new List<SysAdmin>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"select * from Users where Role = 4", conn);
                    await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        string email = reader.GetString("Email");

                        SysAdmin sysAdmin = new SysAdmin { ID = id, Email = email };
                        sysAdmins.Add(sysAdmin);
                    }
                    reader.Close();
                }
                catch
                {

                }
            }
            return sysAdmins;
        }

        public Task<SysAdmin> SearchByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(SysAdmin newSysAdmin)
        {
            try
            {
                await userService.ValidateUpdate(newSysAdmin);
                using SqlConnection conn = new SqlConnection(connectionString);
                {
                    SqlCommand command = new SqlCommand(@"UPDATE Users SET Email = @Email, Password = @Password WHERE ID = @ID", conn);
                    await command.Connection.OpenAsync();
                    command.Parameters.AddWithValue("@ID", newSysAdmin.ID);
                    command.Parameters.AddWithValue("@Email", newSysAdmin.Email);
                    command.Parameters.AddWithValue("@Password", newSysAdmin.Password);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw;
            }
        }
        #endregion
    }
}
