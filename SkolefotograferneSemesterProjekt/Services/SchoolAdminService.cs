using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolAdminService : Connection, ISchoolAdminService
    {
        private IUserService _userService = new UserService();
        private ISchoolService _schoolService = new SchoolService();




        private string _getAllSql = "SELECT Users.ID, Users.Email, SchoolAdmin.PhoneNumber, SchoolAdmin.ContactPerson, SchoolAdmin.SchoolID " +
                                    "FROM Users INNER JOIN SchoolAdmin on Users.ID = SchoolAdmin.ID";

        public Dictionary<string, string> FilterableColumns { get; } = new Dictionary<string, string>
        {
            { "ID", "ID" },
            { "Email", "Email" },
            { "PhoneNumber", "PhoneNumber" },
            { "ContactPerson", "ContactPerson" },
            { "SchoolID", "SchoolID" },
            { "Name", "School Name" },
            { "Street", "School Street" },
            { "ZipCode", "School ZipCode" },
            { "Country", "School Country" },
            { "StudentCount", "School Student Count" }
        };

        public Dictionary<string, string> SortableColumns { get; } = new Dictionary<string, string>
        {
            { "ID", "ID" },
            { "Email", "Email" },
            { "PhoneNumber", "PhoneNumber" },
            { "ContactPerson", "ContactPerson" },
            { "Name", "School Name" },
            { "Street", "School Street" },
            { "ZipCode", "School ZipCode" },
            { "StudentCount", "School Student Count" }
        };


        public async Task Add(SchoolAdmin schoolAdmin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    int userID = await _userService.Add(conn, schoolAdmin);

                    var cmd = new SqlCommand(@"
                INSERT INTO SchoolAdmin (ID, PhoneNumber, ContactPerson, SchoolID)
                VALUES (@ID, @PhoneNumber, @ContactPerson, @SchoolID);
                ", conn);

                    cmd.Parameters.AddWithValue("@ID", userID);
                    cmd.Parameters.AddWithValue("@PhoneNumber", schoolAdmin.PhoneNumber);
                    cmd.Parameters.AddWithValue("@ContactPerson", schoolAdmin.ContactPerson);
                    cmd.Parameters.AddWithValue("@SchoolID", schoolAdmin.TheSchool.ID);

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public async Task<List<SchoolAdmin>> GetAll()
        {
            List<SchoolAdmin> schoolAdmins = new List<SchoolAdmin>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(_getAllSql, conn);
                    await cmd.Connection.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("ID");
                        string email = reader.GetString("Email");
                        string phoneNumber = reader.GetString("PhoneNumber");
                        string contactPerson = reader.GetString("ContactPerson");
                        int schoolID = reader.GetInt32("SchoolID");

                        SchoolAdmin schoolAdmin = new SchoolAdmin { ID = id, Email = email, PhoneNumber = phoneNumber, ContactPerson = contactPerson };
                        School school = await _schoolService.GetById(schoolID);
                        schoolAdmin.TheSchool = school;
                        schoolAdmins.Add(schoolAdmin);
                    }
                    reader.Close();
                }
                catch
                {

                }
            }
            return schoolAdmins;
        }

        public async Task<List<SchoolAdmin>> GetAll(string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            List<SchoolAdmin> schoolAdmins = new List<SchoolAdmin>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                IEnumerable<string> validColumns = FilterableColumns.Keys;

                if (string.IsNullOrWhiteSpace(sortColumn))
                {
                    sortColumn = "ID";
                }
                if (string.IsNullOrWhiteSpace(filterColumn))
                {
                    filterColumn = "All";
                }

                if ((!validColumns.Contains(filterColumn) && filterColumn != "All") || !validColumns.Contains(sortColumn))
                {
                    throw new ArgumentException("Invalid column name");
                }

                try
                {
                    await conn.OpenAsync();
                    string query = "Select Users.ID, Users.Email, SchoolAdmin.ContactPerson, SchoolAdmin.PhoneNumber, SchoolAdmin.SchoolID, School.[Name], School.Street, School.ZipCode, School.Country, School.StudentCount " +
                        "FROM Users " +
                        "INNER JOIN SchoolAdmin ON Users.ID = SchoolAdmin.ID " +
                        "INNER JOIN School ON SchoolAdmin.SchoolID = School.ID";
                    if (!string.IsNullOrWhiteSpace(filterValue))
                    {
                        if (filterColumn == "All")
                        {
                            query += " WHERE " + string.Join(" OR ", validColumns.Select(col => $"{col} LIKE @FilterValue"));
                        }
                        else
                        {
                            query += $" WHERE {filterColumn} LIKE @FilterValue";
                        }
                    }
                    query += $" ORDER BY {sortColumn} {sortOrder}";


                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@FilterValue", $"%{filterValue}%");
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        SchoolAdmin schoolAdmin = SchoolAdminReader(reader);
                        schoolAdmins.Add(schoolAdmin);
                    }
                    reader.Close();
                }
                catch
                {
                }

            }
            return schoolAdmins;
        }

        public async Task<SchoolAdmin> GetById(int id)
        {
            SchoolAdmin schoolAdmin = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM SchoolAdmin sa join Users u on u.ID = sa.ID WHERE u.ID = @ID", conn);
                    await cmd.Connection.OpenAsync();
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        schoolAdmin = SchoolAdminReader(reader);
                    }
                    reader.Close();
                }
                catch
                {

                }
            }
            return schoolAdmin;
        }

        public Task Update(SchoolAdmin schoolAdmin)
        {
            throw new NotImplementedException();
        }

        private SchoolAdmin SchoolAdminReader(SqlDataReader reader)
        {
            int id = reader.GetInt32("ID");
            string email = reader.GetString("Email");
            string phoneNumber = reader.GetString("PhoneNumber");
            string contactPerson = reader.GetString("ContactPerson");
            int schoolID = reader.GetInt32("SchoolID");
            School school = _schoolService.GetById(schoolID).Result;
            SchoolAdmin schoolAdmin = new SchoolAdmin { ID = id, Email = email, PhoneNumber = phoneNumber, ContactPerson = contactPerson, TheSchool = school };
            return schoolAdmin;
        }
    }
}
