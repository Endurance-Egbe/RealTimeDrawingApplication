using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        //Navigation Property
        public virtual AccountModel User { get; set; }
        public virtual List<ShareUserProject> ShareUsers { get; set; }
        public virtual List<DrawingComponentModel> ShapeComponents { get; set; }
        //public ProjectModel()
        //{
        //    ShareUsers = new List<ShareUserProject>();
        //    ShapeComponents = new List<DrawingComponentModel>();
        //}

    }
}
