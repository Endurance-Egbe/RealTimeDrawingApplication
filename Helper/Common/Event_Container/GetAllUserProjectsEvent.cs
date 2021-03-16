using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows;

namespace View.Helper.Common.Event_Container
{
    public class GetAllUserProjectsEvent:PubSubEvent<ObservableCollection<string>>
    {
    }
}
