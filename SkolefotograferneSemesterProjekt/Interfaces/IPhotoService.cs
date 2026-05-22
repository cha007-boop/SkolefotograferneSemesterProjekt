using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotoService
    {
        /// <summary>
        /// All the columns from the photo table the GetAll method can sort by, with the key being the column name in the database, and the value being the display name for that column.
        /// </summary>
        Dictionary<string, string> SortableColumns { get; }
        /// <summary>
        /// All the columns the GetAll method can filter by, with the key being the column name in the database, and the value being the display name for that column. This can also include columns from other tables that are related to the photo table, such as student name or class name, as long as they are included in the SQL query in the PhotoService class.
        /// </summary>
        Dictionary<string, string> FilterableColumns { get; }
        /// <summary>
        /// Method for adding a photo to the database.
        /// </summary>
        /// <param name="photo">The photo to add</param>
        /// <returns>A task representing the asynchronous operation, containing the filename of the added photo</returns>
        Task<string> Add(Photo photo);
        /// <summary>
        /// Method for getting a photo by its filename.
        /// </summary>
        /// <param name="filename">The filename of the photo</param>
        /// <returns>A task representing the asynchronous operation, containing the photo if found</returns>
        Task<Photo> GetByFilename(string filename);
        /// <summary>
        /// Method for getting all photos from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all photos</returns>
        Task<List<Photo>> GetAll();
        /// <summary>
        /// Method for getting class photos by the school class ID.
        /// </summary>
        /// <param name="schoolClassId">The id of the school class to get photos of</param>
        /// <returns>A task representing the asynchronous operation, containing a list of photos for the specified school class</returns>
        Task<List<Photo>> GetClassPhotosByClassId(int schoolClassId);
        /// <summary>
        /// Method for getting portrait photos by the student ID. This should return all portraits of the student.
        /// </summary>
        /// <param name="studentId">The id of the student to get portraits of</param>
        /// <returns>A task representing the asynchronous operation, containing a list of portrait photos for the specified student</returns>
        Task<List<Photo>> GetPortraitsByStudentId(int studentId);
        /// <summary>
        /// Method for getting all photos from a specific photo event by the photo event ID.
        /// </summary>
        /// <param name="photoEventId">The id of the photo event to get photos of</param>
        /// <returns>A task representing the asynchronous operation, containing a list of photos for the specified photo event</returns>
        Task<List<Photo>> GetByPhotoEventId(int photoEventId);
        /// <summary>
        /// Method for removing a photo from the database.
        /// </summary>
        /// <param name="filename">The filename of the photo to remove</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task RemovePhoto(string filename);
        /// <summary>
        /// Method for searching photos based on a filter column, filter value, sort column, sort order, and optional conditions.
        /// </summary>
        /// <param name="filterColumn">The column to filter by</param>
        /// <param name="filterValue">The value to filter by</param>
        /// <param name="sortColumn">The column to sort by</param>
        /// <param name="sortOrder">The order to sort by (ascending or descending)</param>
        /// <param name="conditions">Additional conditions for the search</param>
        /// <returns>A task representing the asynchronous operation, containing a list of photos that match the specified criteria</returns>
        Task<List<Photo>> Search(string filterColumn, string filterValue, string sortColumn, string sortOrder, List<string> conditions = null);
    }
}
