using View.Models;

namespace View.ViewModels.DatabaseServices
{
    public interface IAccountRepository
    {
        void Create(AccountModel account);
        AccountModel GetByEmail(string email);
    }
}
