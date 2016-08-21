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
using Deadfile.ViewModel;

namespace Deadfile.Persons
{
    public class PersonsViewModel : PageViewModel
    {
        public PersonDirectoryViewModel PersonDirectoryViewModel { get; private set; }
        public PersonDetailsViewModel PersonDetailsViewModel { get; private set; }

        public PersonsViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            PersonDirectoryViewModel = new PersonDirectoryViewModel(personService, dispatcher, aggregator, dialogService);
            PersonDetailsViewModel = new PersonDetailsViewModel(personService, dispatcher, aggregator, dialogService);
        }

        public override string Name { get { return "Persons"; } }

        public override Task StartTask()
        {
            return PersonDirectoryViewModel.RefreshAsync();
        }

    }
}
