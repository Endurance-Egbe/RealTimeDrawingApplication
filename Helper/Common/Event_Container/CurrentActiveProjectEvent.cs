using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.ProxyModel;

namespace View.Helper.Common.Event_Container
{
    public class CurrentActiveProjectEvent:PubSubEvent<ProjectProxyModel>
    {
    }
}
