using System;

namespace Glitch9.Game
{
    public static class DateTimeId
    {
        public static int CreateNew()
        {
            DateTime now = DateTime.UtcNow;
            int id = now.Year % 100; // Last two digits of the year
            id = id * 12 + now.Month - 1; // 0 to 11 for months
            id = id * 31 + now.Day - 1; // 0 to 30 for days
            id = id * 24 + now.Hour; // 0 to 23 for hours
            id = id * 60 + now.Minute; // 0 to 59 for minutes
            id = id * 60 + now.Second; // 0 to 59 for seconds
            return id;
        }
    }
}