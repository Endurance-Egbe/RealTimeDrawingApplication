namespace View.ViewModels.DatabaseServices
{
    public static class ShareUserServiceProvider
    {
        public static ShareUserService Current { get; private set; } =
            new ShareUserService(new SqliteShareUserRepository());

        public static void SetCurrent(ShareUserService service)
        {
            Current = service;
        }
    }
}
