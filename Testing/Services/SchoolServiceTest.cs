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
        public async Task TestAddAsync()
        {
            // Arrange
            int countBeforeAdd = (await _schoolService.GetAll()).Count;

            School newSchool = new School
            {
                Name = $"Test_{Guid.NewGuid()}",
                Country = "Testland",
                Street = "Test Street",
                ZipCode = "12345",
                StudentCount = 100
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

        [TestMethod]
        public async Task TestDelete()
        {
            // Arrange
            School newSchool = new School
            {
                Name = $"Test_{Guid.NewGuid()}",
                Country = "Testland",
                Street = "Test Street",
                ZipCode = "12345",
                StudentCount = 100
            };
            int idToDelete = 0;

            await _schoolService.Add(newSchool);
            List<School> schools = await _schoolService.GetAll();
            idToDelete = schools.Last().ID;
            int countBeforeDelete = schools.Count;
            // Act
            await _schoolService.Delete(idToDelete);
            // Assert
            schools = await _schoolService.GetAll();
            Assert.AreEqual(countBeforeDelete - 1, schools.Count);
            Assert.IsFalse(schools.Any(s => s.ID == idToDelete));
        }

        [TestMethod]
        public async Task TestGetById()
        {
            // Arrange
            School newSchool = new School
            {
                Name = $"Test_{Guid.NewGuid()}",
                Country = "Testland",
                Street = "Test Street",
                ZipCode = "12345",
                StudentCount = 100
            };
            int idToGet = 0;
            await _schoolService.Add(newSchool);
            List<School> schools = await _schoolService.GetAll();
            idToGet = schools.Last().ID;
            try
            {
                // Act
                School schoolFromDb = await _schoolService.GetById(idToGet);
                // Assert
                Assert.IsNotNull(schoolFromDb);
                Assert.AreEqual(idToGet, schoolFromDb.ID);
                Assert.AreEqual(newSchool.Name, schoolFromDb.Name);
                Assert.AreEqual(newSchool.Country, schoolFromDb.Country);
                Assert.AreEqual(newSchool.Street, schoolFromDb.Street);
                Assert.AreEqual(newSchool.ZipCode, schoolFromDb.ZipCode);
                Assert.AreEqual(newSchool.StudentCount, schoolFromDb.StudentCount);
            }
            finally
            {
                if (idToGet > 0)
                {
                    await _schoolService.Delete(idToGet);
                }
            }
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            // Arrange
            School newSchool = new School
            {
                Name = $"Test_{Guid.NewGuid()}",
                Country = "Testland",
                Street = "Test Street",
                ZipCode = "12345",
                StudentCount = 100
            };
            int idToUpdate = 0;
            await _schoolService.Add(newSchool);
            List<School> schools = await _schoolService.GetAll();
            idToUpdate = schools.Last().ID;
            try
            {
                // Act
                School updatedSchool = new School
                {
                    ID = idToUpdate,
                    Name = $"Updated_{Guid.NewGuid()}",
                    Country = "Updatedland",
                    Street = "Updated Street",
                    ZipCode = "54321",
                    StudentCount = 200
                };
                await _schoolService.Update(updatedSchool);
                // Assert
                schools = await _schoolService.GetAll();
                School schoolFromDb = schools.FirstOrDefault(s => s.ID == idToUpdate);
                Assert.IsNotNull(schoolFromDb);
                Assert.AreEqual(updatedSchool.Name, schoolFromDb.Name);
                Assert.AreEqual(updatedSchool.Country, schoolFromDb.Country);
                Assert.AreEqual(updatedSchool.Street, schoolFromDb.Street);
                Assert.AreEqual(updatedSchool.ZipCode, schoolFromDb.ZipCode);
                Assert.AreEqual(updatedSchool.StudentCount, schoolFromDb.StudentCount);
            }
            finally
            {
                if (idToUpdate > 0)
                {
                    await _schoolService.Delete(idToUpdate);
                }
            }
        }
    }
}