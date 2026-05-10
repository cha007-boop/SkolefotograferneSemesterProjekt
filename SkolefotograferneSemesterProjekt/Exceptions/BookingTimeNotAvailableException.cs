namespace SkolefotograferneSemesterProjekt.Exceptions
{
    public class BookingTimeNotAvailableException : Exception
    {
        public BookingTimeNotAvailableException(string message) 
            : base(message)
        {
        }
    }
}
