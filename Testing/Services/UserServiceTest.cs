using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Exceptions;
using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Models;
using SkolefotograferneSemesterProjekt.Services;

namespace Testing.Services
{
    [TestClass]
    [DoNotParallelize]
    public sealed class UserServiceTest
    {
        private IUserService _userService = new UserService();
        [TestMethod]
        public async Task TestAddSuccesfulAsync()
        {
            // Arrange
            int countBeforeAdd = (await _userService.GetAll()).Count;

            User newUser = new User
            {
                Email = $"Test_{Guid.NewGuid()}",
                Password = "Test123",
                Role = UserRole.SysAdmin
            };

            int newestId = 0;

            try
            {
                // Act
                newestId = await _userService.Add(newUser);

                // Assert
                List<User> users = await _userService.GetAll();

                Assert.AreEqual(countBeforeAdd + 1, users.Count);
                Assert.AreEqual(newestId, users.Last().ID);
            }
            finally
            {
                if (newestId > 0)
                {
                    await _userService.Delete(newestId);
                }
            }
        }
        [TestMethod]
        public async Task TestAddThrowsEmailTakenExceptionAsync()
        {
            // Arrange
            int countBeforeAdd = (await _userService.GetAll()).Count;
            User newUserTakenMail = new User
            {
                Email = "admin",
                Password = "Test123",
                Role = UserRole.SysAdmin
            };

            // Act

            await Assert.ThrowsExceptionAsync<TakenMailException>(async () =>
            await _userService.Add(newUserTakenMail));

            Assert.AreEqual(countBeforeAdd, (await _userService.GetAll()).Count);
        }

        [TestMethod]
        public async Task TestAddThrowsPasswordTooShortExceptionAsync()
        {
            // Arrange
            User newUserShortPassword = new User
            {
                Email = $"Test_{Guid.NewGuid()}",
                Password = "123",
                Role = UserRole.SysAdmin
            };

            int countBeforeAdd = (await _userService.GetAll()).Count;

            // Act

            await Assert.ThrowsExceptionAsync<PasswordTooShortException>(async () =>
            await _userService.Add(newUserShortPassword));

            Assert.AreEqual(countBeforeAdd, (await _userService.GetAll()).Count);
        }

        [TestMethod]
        public async Task TestDeleteSuccessfulAsync()
        {
            // Arrange
            User newUser = new User
            {
                Email = $"Test_{Guid.NewGuid()}",
                Password = "Test123",
                Role = UserRole.SysAdmin
            };
            int newestId = 0;

            newestId = await _userService.Add(newUser);

            int countBeforeDelete = (await _userService.GetAll()).Count;

            try
            {
                // Act
                await _userService.Delete(newestId);

            }
            finally
            {
                // Assert
                List<User> users = await _userService.GetAll();
                Assert.AreEqual(countBeforeDelete - 1, users.Count);
                Assert.IsFalse(users.Any(u => u.ID == newestId));
            }

        }
        [TestMethod]
        public async Task DeleteSuccessfulOnOtherUserTypesAsync()
        {
            // Arrange
            Parent parent = new Parent
            {
                Email = $"Parent_{Guid.NewGuid()}",
                Password = "test123",
                FirstName = "Test",
                Surname = "Parent",
                PhoneNumber = "12345678",
                Role = UserRole.Parent
            };

            IParentServices parentService = new ParentServices();

            await parentService.AddParent(parent);
            int parentId = (await parentService.GetAllParents()).Last().ID;

            int userCountBeforeDelete = (await _userService.GetAll()).Count;
            int parentCountBeforeDelete = (await parentService.GetAllParents()).Count;

            // Act
            try
            {
                await _userService.Delete(parentId);
            }
            finally
            {
                // Assert
                List<User> users = await _userService.GetAll();
                List<Parent> parents = await parentService.GetAllParents();
                Assert.AreEqual(userCountBeforeDelete - 1, users.Count);
                Assert.AreEqual(parentCountBeforeDelete - 1, parents.Count);
                Assert.IsFalse(users.Any(u => u.Email == parent.Email));
                Assert.IsFalse(parents.Any(p => p.Email == parent.Email));
            }
        }
    }
}
