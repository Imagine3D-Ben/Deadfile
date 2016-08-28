using Deadfile.Data;
using Deadfile.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using ObservableImmutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.ViewModel
{
    public abstract class TwoColumnPageViewModel : PageViewModel
    {
        public TwoColumnPageViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {

        }

        public abstract ViewModelBase LeftControlViewModel { get; }

        public abstract ViewModelBase RightControlTopViewModel { get; }

        public abstract ViewModelBase RightControlBottomViewModel { get; }

    }
}
