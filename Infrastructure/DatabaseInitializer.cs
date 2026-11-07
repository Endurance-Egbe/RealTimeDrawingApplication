using System;

namespace View.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            try
            {
                using (var context = new SQLiteEFCore())
                {
                    context.Database.EnsureCreated();
                }
            }
            catch (Exception exception) when (!(exception is ApplicationOperationException))
            {
                throw new ApplicationOperationException(
                    "The application database could not be initialized. Check the configured database connection.",
                    exception);
            }
        }
    }
}
