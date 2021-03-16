using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModels.ProxyModel
{
    public class AccountProxyModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public List<ProjectProxyModel> ProjectProxyModels { get; set; }
    }
}
