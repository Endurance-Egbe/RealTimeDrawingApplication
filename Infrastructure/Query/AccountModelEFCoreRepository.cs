using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Models;

namespace View.Infrastructure
{
    public class AccountModelEFCoreRepository : EFCoreRepository<AccountModel>
    {
        private UserDbContext userContext;
        public AccountModelEFCoreRepository(UserDbContext _userContext) : base(_userContext)
        {
            userContext = _userContext;
        }
        public new AccountModel GetModel(string email)
        {

            AccountModel model = userContext.Set<AccountModel>().FirstOrDefault(x => x.Email == email);

            return model;
        }
        
    }

}
