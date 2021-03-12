using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }


        //Navigation Property
        public virtual List<ProjectModel> Projects { get; set; }
        //public AccountModel()
        //{
        //    Projects = new List<ProjectModel>();
        //}
    }
}
