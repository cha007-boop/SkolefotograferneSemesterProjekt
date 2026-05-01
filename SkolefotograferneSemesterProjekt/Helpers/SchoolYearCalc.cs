namespace SkolefotograferneSemesterProjekt.Helpers
{
    public static class SchoolYearCalc
    {
        public static string GetSchoolYear()
        {
            int year = DateTime.Now.Year;
            if (DateTime.Now.Month < 8)
            {
                year -= 1;
            }
            return year.ToString();
        }
    }
}
