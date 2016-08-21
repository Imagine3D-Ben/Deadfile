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
using Deadfile.Whatever;
using Deadfile.Persons;

namespace Deadfile.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<PageViewModel> PageViewModels { get; private set; }

        public ICommand ChangePageCommand { get; private set; }

        public ICommand AppStartCommand { get; private set; }

        public MainWindowViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {
            PageViewModels = new List<ViewModel.PageViewModel>();
            PageViewModels.Add(new PersonsViewModel(personService, dispatcher, aggregator, dialogService));
            PageViewModels.Add(new HomeViewModel(personService, dispatcher, aggregator, dialogService));
            PageViewModels.Add(new WhateverViewModel(personService, dispatcher, aggregator, dialogService));
            CurrentPageViewModel = PageViewModels[0];
            ChangePageCommand = new RelayCommand(p => ChangeViewModel((PageViewModel)p), p => p is PageViewModel);

            AppStartCommand = new AsyncCommand(() => Task.WhenAll(PageViewModels.Select((vm) => vm.StartTask()).ToArray()));
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
    }
}
