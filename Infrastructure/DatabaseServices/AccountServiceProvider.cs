namespace View.ViewModels.DatabaseServices
{
    public static class AccountServiceProvider
    {
        public static AccountService Current { get; } =
            new AccountService(new SqliteAccountRepository());
    }
}
