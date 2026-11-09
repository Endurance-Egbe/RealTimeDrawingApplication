using View.Infrastructure;
using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public class SqliteAccountRepository : IAccountRepository
    {
        public void Create(AccountModel account)
        {
            using (var sqlite = new SQLiteEFCore())
            {
                new AccountModelEFCoreRepository(sqlite).CreateModel(account);
            }
        }

        public AccountModel GetByEmail(string email)
        {
            using (var sqlite = new SQLiteEFCore())
            {
                return new AccountModelEFCoreRepository(sqlite).GetModelByEmail(email);
            }
        }
    }
}
