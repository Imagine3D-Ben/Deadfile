using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deadfile.Data;
using Deadfile.Events;
using Deadfile.Helpers;
using Deadfile.Model;
using Deadfile.Services;
using Deadfile.ViewModel;
using ObservableImmutable;

namespace Deadfile.Persons
{
    public class PersonButtonsViewModel : ViewModelBase
    {
        Person person;

        public ICommand NewPersonCommand { get; private set; }
        public ICommand EditPersonCommand { get; private set; }
        public ICommand DeletePersonCommand { get; private set; }

        public PersonButtonsViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService, ICommand newPersonCommand, ICommand editPersonCommand, ICommand deletePersonCommand)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            NewPersonCommand = newPersonCommand;
            EditPersonCommand = editPersonCommand;
            DeletePersonCommand = deletePersonCommand;
        }
    }
}
