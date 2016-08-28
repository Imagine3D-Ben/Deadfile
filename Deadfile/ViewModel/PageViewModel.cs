using Deadfile.Data;
using Deadfile.Helpers;
using Deadfile.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using ObservableImmutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Deadfile.ViewModel
{
    public abstract class PageViewModel : ViewModelBase
    {
        public PageViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService,dispatcher, aggregator, dialogService)
        {
        }

        public abstract string Name { get; }

        public abstract Task StartTask();
    }
}
