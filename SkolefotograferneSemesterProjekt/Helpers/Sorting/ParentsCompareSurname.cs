using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Helpers.Sorting
{
    public class ParentsCompareSurname : IComparer<Parent>
    {
        public int Compare(Parent? x, Parent? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return string.Compare(x.Surname, y.Surname, StringComparison.OrdinalIgnoreCase);
        }
    }
}
