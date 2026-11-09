namespace View.ViewModels.DatabaseServices
{
    public static class AccountServiceProvider
    {
        public static AccountService Current { get; private set; } =
            new AccountService(new SqliteAccountRepository());

        public static void SetCurrent(AccountService accountService)
        {
            Current = accountService;
        }
    }
}
