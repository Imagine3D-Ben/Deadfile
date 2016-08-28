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
using ObservableImmutable;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Deadfile.Persons
{
    public class PersonsViewModel : TwoColumnPageViewModel
    {
        public PersonDirectoryViewModel PersonDirectoryViewModel { get; private set; }
        public PersonDetailsViewModel PersonDetailsViewModel { get; private set; }
        public PersonButtonsViewModel PersonButtonsViewModel { get; private set; }

        public PersonsViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            PersonDirectoryViewModel = new PersonDirectoryViewModel(personService, dispatcher, aggregator, dialogService);
            PersonDetailsViewModel = new PersonDetailsViewModel(personService, dispatcher, aggregator, dialogService);
            PersonButtonsViewModel = new PersonButtonsViewModel(personService, dispatcher, aggregator, dialogService, PersonDetailsViewModel.NewPersonCommand, PersonDetailsViewModel.EditPersonCommand, PersonDetailsViewModel.DeletePersonCommand);
        }

        public override string Name { get { return "Persons"; } }

        public override Task StartTask()
        {
            return PersonDirectoryViewModel.RefreshAsync();
        }

        public override ViewModelBase LeftControlViewModel
        {
            get
            {
                return PersonDirectoryViewModel;
            }
        }

        public override ViewModelBase RightControlTopViewModel
        {
            get
            {
                return PersonDetailsViewModel;
            }
        }

        public override ViewModelBase RightControlBottomViewModel
        {
            get
            {
                return PersonButtonsViewModel;
            }
        }
    }
}
