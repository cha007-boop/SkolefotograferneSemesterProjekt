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
    public class ParentServiceTest
    {
        private IParentServices _parentService = new ParentServices();

        [TestMethod]
        public async Task TestAddParentMethod()
        {
            //Arrange
            int countBefore = (await _parentService.GetAllParents()).Count;
            Parent newParent = new Parent
            {
                FirstName = "John",
                Surname = "Doe",
                PhoneNumber = "12345678",
                Email = $"Test{Guid.NewGuid()}@example.com", // Ensure unique email for each test run
                Password = "password123"
            };

            //Act
            newParent.ID = await _parentService.AddParent(newParent);
            //Assert
            Assert.AreEqual(countBefore + 1, (await _parentService.GetAllParents()).Count);


            // Clean up
            await _parentService.DeleteParent(newParent);
        }
    }
}
