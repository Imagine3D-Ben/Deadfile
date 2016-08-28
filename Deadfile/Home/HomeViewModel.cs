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
        private ITaskScheduler taskScheduler;

        public HomeViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService, ITaskScheduler taskScheduler)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            this.taskScheduler = taskScheduler;
        }

        public override string Name
        {
            get
            {
                return "Home Page";
            }
        }

        public override void StartTask()
        {
        }
    }
}
