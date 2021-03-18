using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.ViewModels;

namespace View.Helper.Common.Event_Container
{
    public class GetAllShareUserEvent:PubSubEvent<ObservableCollection<Users>>
    {
    }
}
