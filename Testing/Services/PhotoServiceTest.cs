using SkolefotograferneSemesterProjekt.Interfaces;
using SkolefotograferneSemesterProjekt.Services;
using SkolefotograferneSemesterProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Services
{
    [TestClass]
    [DoNotParallelize]
    public sealed class PhotoServiceTest
    {
        private IPhotoService _photoService = new PhotoService();
        [TestMethod]
        public async Task TestAdd()
        {
            // Arrange
            int countBeforeAdd = (await _photoService.GetAll()).Count;
            Photo newPhoto = new Photo
            {
                Filename = $"Test_{Guid.NewGuid()}",
                ThePhotoEvent = null,
                Child = null,
                TheSchoolClass = null,
                UploadedAt = DateTime.Now
            };
            string filenameAdded = newPhoto.Filename;
            try
            {
                // Act
                await _photoService.Add(newPhoto);
                // Assert
                List<Photo> photos = (await _photoService.GetAll()).OrderBy(p => p.UploadedAt).ToList();


                Assert.AreEqual(countBeforeAdd + 1, photos.Count);
                Assert.AreEqual(filenameAdded, photos.Last().Filename);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filenameAdded))
                {
                    await _photoService.RemovePhoto(filenameAdded);
                }
            }
        }

        [TestMethod]
        public async Task TestRemove()
        {
            // Arrange
            Photo newPhoto = new Photo
            {
                Filename = $"Test_{Guid.NewGuid()}",
                Child = null,
                TheSchoolClass = null,
                UploadedAt = DateTime.Now
            };
            string filenameAdded = await _photoService.Add(newPhoto);
            int countBeforeRemove = (await _photoService.GetAll()).Count;

            // Act
            await _photoService.RemovePhoto(filenameAdded);
            // Assert
            List<Photo> photos = await _photoService.GetAll();
            Assert.AreEqual(countBeforeRemove - 1, photos.Count);
            Assert.IsFalse(photos.Any(p => p.Filename == filenameAdded));

        }

        [TestMethod]
        public async Task TestGetByFilename()
        {
            // Arrange
            Photo newPhoto = new Photo
            {
                Filename = $"Test_{Guid.NewGuid()}",
                ThePhotoEvent = null,
                Child = null,
                TheSchoolClass = null,
                UploadedAt = DateTime.Now
            };
            string filenameAdded = await _photoService.Add(newPhoto);
            try
            {
                // Act
                Photo? retrievedPhoto = await _photoService.GetByFilename(filenameAdded);
                // Assert
                Assert.IsNotNull(retrievedPhoto);
                Assert.AreEqual(filenameAdded, retrievedPhoto.Filename);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(filenameAdded))
                {
                    await _photoService.RemovePhoto(filenameAdded);
                }
            }
        }
    }
}
