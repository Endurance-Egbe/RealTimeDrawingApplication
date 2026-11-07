using System;

namespace View.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var context = new SQLiteEFCore())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
