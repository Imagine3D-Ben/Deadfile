using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Deadfile.Data;
using Deadfile.Events;
using Deadfile.Helpers;
using Deadfile.Model;
using Deadfile.Services;
using Deadfile.ViewModel;
using System.Windows.Data;
using System.ComponentModel;
using ObservableImmutable;

namespace Deadfile.Persons
{
    public class PersonDirectoryViewModel : ViewModelBase
    {
        readonly ObservableImmutableList<Person> personDirectory;
        Person selectedPerson;
        readonly ITaskScheduler taskScheduler;

        public PersonDirectoryViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService, IChubbFactory chubFactory, ITaskScheduler taskScheduler) 
            : base(personService, dispatcher, aggregator, dialogService)
        {
            personDirectory = new ObservableImmutableList<Person>(dispatcher, chubFactory);
            this.taskScheduler = taskScheduler;
            aggregator.GetEvent<PersonDirectoryUpdatedEvent>().Subscribe(OnPersonDirectoryUpdated, dispatcher.BackgroundThread());
            PersonDirectoryLinks = new CollectionViewSource();
            PersonDirectoryLinks.Source = InnerPersonDirectory;
            PersonDirectoryLinks.Filter += ApplyFilter;
            aggregator.GetEvent<PersonDeletedEvent>().Subscribe(OnPersonDeleted, dispatcher.UIThread());
            aggregator.GetEvent<PersonFilterEvent>().Subscribe((s) => PersonDirectoryLinks.View.Refresh(), dispatcher.UIThread());
        }

        void ApplyFilter(object sender, FilterEventArgs e)
        {
            //each item is a specific object
            Person si = (Person)e.Item;
            if (PersonFilter == null)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = si.FullName.ToUpper().Contains(PersonFilter.ToUpper());
            }
        }

        internal ObservableImmutableList<Person> InnerPersonDirectory
        {
            get
            {
                return this.personDirectory;
            }
        }

        internal CollectionViewSource PersonDirectoryLinks { get; set; }

        public ICollectionView PersonDirectory
        {
            get { return PersonDirectoryLinks.View; }
        }

        private string personFilter = "";
        public string PersonFilter
        {
            get
            {
                return personFilter;
            }
            set
            {
                if (this.personFilter != value)
                {
                    this.personFilter = value;
                    OnPropertyChanged("PersonFilter");
                    aggregator.GetEvent<PersonFilterEvent>().Publish(this.personFilter);
                }
            }
        }

        public Person SelectedPerson
        {
            get
            {
                return this.selectedPerson;
            }
            set
            {
                if (this.selectedPerson != value)
                {
                    this.selectedPerson = value;
                    OnPropertyChanged("SelectedPerson");
                    aggregator.GetEvent<SelectedPersonChangeEvent>().Publish(this.selectedPerson);
                }
            }
        }

        public void RefreshAsync()
        {
            IsBusy = true;
            try
            {
                var persons = this.personService.GetPersons();
                personDirectory.DoOperation((items) =>
                    {
                        return items.Clear().AddRange(persons.Where((p) => p.FullName.ToUpper().Contains(personFilter)));
                    });
            }
            catch (Exception ex)
            {
                //TODO: Publish operation failed
                throw new ApplicationException("Exception failed in refresh", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnPersonDirectoryUpdated(object state)
        {
            RefreshAsync();
        }

        private void OnPersonDeleted(Person person)
        {
            this.personDirectory.Remove(person);
        }
    }
}
