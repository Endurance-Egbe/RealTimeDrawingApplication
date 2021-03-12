using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels.ProxyModel;

namespace View.ViewModels.Common.Event_Container
{
    public class JsonImportedProxiesEvent:PubSubEvent<IEnumerable<DrawingComponentProxyModel>>
    {
    }
}
