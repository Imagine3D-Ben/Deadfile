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

namespace Deadfile.Whatever
{
    public class WhateverViewModel : PageViewModel
    {
        public WhateverViewModel(IPersonService personService, IDispatcher dispatcher, IEventAggregator aggregator, IDialogService dialogService)
            : base(personService, dispatcher, aggregator, dialogService)
        {

        }

        public override string Name
        {
            get
            {
                return "Whatever";
            }
        }

        public override Task StartTask()
        {
            return Task.Run(() => { });
        }
    }
}
