using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Commands;
using Deadfile.Data;
using Deadfile.Helpers;
using Deadfile.Services;

namespace Deadfile.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public PersonDirectoryViewModel PersonDirectoryViewModel { get; private set; }
        public PersonDetailsViewModel PersonDetailsViewModel { get; private set; }

        public ICommand AppStartCommand { get; private set; }

        public MainWindowViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            PersonDirectoryViewModel = new PersonDirectoryViewModel(personService, dispatcher, aggregator, dialogService);
            PersonDetailsViewModel = new PersonDetailsViewModel(personService, dispatcher, aggregator, dialogService);

            AppStartCommand = new AsyncCommand(PersonDirectoryViewModel.RefreshAsync);
        }
    }
}
