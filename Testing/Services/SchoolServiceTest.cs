using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Services
{
    [TestClass]
    [DoNotParallelize]
    public sealed class SchoolServiceTest
    {
        private ISchoolService _schoolService = new SchoolService();
        [TestMethod]
        public async Task TestAddSync()
        {
            // Arrange
            int countBeforeAdd = (await _schoolService.GetAll()).Count;

            School newSchool = new School
            {
                Name = $"Test_{Guid.NewGuid()}", Country = "Testland", Street = "Test Street", ZipCode = "12345", StudentCount = 100
            };

            int newestId = 0;

            try
            {
                // Act
                await _schoolService.Add(newSchool);

                // Assert
                List<School> schools = await _schoolService.GetAll();
                newestId = schools.Last().ID;

                Assert.AreEqual(countBeforeAdd + 1, schools.Count);
                Assert.AreEqual(newestId, schools.Last().ID);
            }
            finally
            {
                if (newestId > 0)
                {
                    await _schoolService.Delete(newestId);
                }
            }
        }
    }
}