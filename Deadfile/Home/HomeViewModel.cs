using Deadfile.Data;
using Deadfile.Helpers;
using Deadfile.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.ViewModel;
using ObservableImmutable;

namespace Deadfile.Home
{
    public class HomeViewModel : PageViewModel
    {
        public HomeViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {

        }

        public override string Name
        {
            get
            {
                return "Home Page";
            }
        }

        public override Task StartTask()
        {
            return Task.Run(() => { });
        }
    }
}
