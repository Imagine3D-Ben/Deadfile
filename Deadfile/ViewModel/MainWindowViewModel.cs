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
using Deadfile.Home;
using Deadfile.Persons;
using ObservableImmutable;

namespace Deadfile.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<PageViewModel> PageViewModels { get; private set; }

        public ICommand ChangePageCommand { get; private set; }

        public ICommand AppStartCommand { get; private set; }

        public MainWindowViewModel(IDeadfileDbService databaseService, IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService, IExitService exitService, IChubbFactory chubFactory, ITaskScheduler taskScheduler)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            PageViewModels = new List<ViewModel.PageViewModel>();
            PageViewModels.Add(new HomeViewModel(personService, dispatcher, aggregator, dialogService, taskScheduler));
            PageViewModels.Add(new PersonsViewModel(personService, dispatcher, aggregator, dialogService, chubFactory, taskScheduler));
            CurrentPageViewModel = PageViewModels[0];
            ChangePageCommand = new RelayCommand(p => ChangeViewModel((PageViewModel)p), p => p is PageViewModel);
            MenuConnect = new RelayCommand(p => exitService.Exit());
            MenuExit = new RelayCommand(p => exitService.Exit());

            AppStartCommand = new AsyncCommand(() => Task.WhenAll(PageViewModels.Select((vm) => Task.Run(new Action(() => vm.StartTask()))).ToArray()));
        }

        private void ChangeViewModel(PageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        private PageViewModel _currentPageViewModel;
        public PageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        public ICommand MenuExit { get; private set; }
        public ICommand MenuConnect { get; private set; }
    }
}
