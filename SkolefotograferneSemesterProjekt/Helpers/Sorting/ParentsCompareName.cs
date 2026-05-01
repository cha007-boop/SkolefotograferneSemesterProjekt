using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Helpers.Sorting
{
    public class ParentsCompareName : IComparer<Parent>
    {
        public int Compare(Parent? x, Parent? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return string.Compare(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
