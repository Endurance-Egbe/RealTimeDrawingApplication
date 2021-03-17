using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models
{
    public class ShareUserProject
    {
        public int ShareUserId { get; set; }
        public string Email { get; set; }
        //Navigation Property
        public virtual AccountModel Users { get; set; }
        public virtual ProjectModel Project { get; set; }
    }
}
