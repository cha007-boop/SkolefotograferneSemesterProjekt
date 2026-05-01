using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using System.Data;

namespace SkolefotograferneSemesterProjekt.Services
{
    public class SchoolService : Connection, ISchoolService
    {
        public Dictionary<string, string> Columns { get; } = new Dictionary<string, string>
        {
            { "ID", "ID" },
            { "Name", "Name" },
            { "Street", "Street" },
            { "ZipCode", "Zip Code" },
            { "Country", "Country" },
            { "StudentCount", "Student Count" }
        };

        public async Task Add(School school)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    var command = new SqlCommand(@"
                    INSERT INTO School (Name, StudentCount, Street, ZipCode, Country)
                    VALUES (@Name, @StudentCount, @Street, @ZipCode, @Country)", conn);

                    command.Parameters.AddWithValue("@Name", school.Name);
                    command.Parameters.AddWithValue("@StudentCount", school.StudentCount);
                    command.Parameters.AddWithValue("@Street", school.Street);
                    command.Parameters.AddWithValue("@ZipCode", school.ZipCode);
                    command.Parameters.AddWithValue("@Country", school.Country);

                    await command.ExecuteNonQueryAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<School>> GetAll()
        {
            List<School> schools = new List<School>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    SqlCommand command = new SqlCommand(@"SELECT * FROM School", conn);

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        School school = SchoolReader(reader);
                        schools.Add(school);
                    }
                    reader.Close();
                }
                catch
                {

                }
            }
            return schools;
        }

        public async Task<School> GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                School school = new School();
                try
                {
                    await conn.OpenAsync();
                    SqlCommand command = new SqlCommand(@"SELECT * FROM School Where ID = @ID", conn);

                    command.Parameters.AddWithValue("@ID", id);

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    await reader.ReadAsync();

                    school = SchoolReader(reader);
                    reader.Close();

                    return school;
                }
                catch
                {

                }
                return school;
            }
        }

        public async Task<List<School>> GetAll(string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                List<School> schools = new List<School>();
                IEnumerable<string> validColumns = Columns.Keys;

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
                    string query = "SELECT * FROM School";
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
                        School school = SchoolReader(reader);
                        schools.Add(school);
                    }
                    reader.Close();
                }
                catch
                {
                }
                return schools;
            }
        }

        public Task Update(School school)
        {
            throw new NotImplementedException();
        }

        private School SchoolReader(SqlDataReader reader)
        {
            int id = reader.GetInt32("ID");
            string name = reader.GetString("Name");
            int studentCount = reader.GetInt32("StudentCount");
            string street = reader.GetString("Street");
            string zipCode = reader.GetString("ZipCode");
            string country = reader.GetString("Country");

            return new School { ID = id, Name = name, StudentCount = studentCount, Street = street, ZipCode = zipCode, Country = country };

        }
    }
}
