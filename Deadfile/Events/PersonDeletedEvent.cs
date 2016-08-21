using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Model;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Deadfile.Events
{
    public class PersonDeletedEvent : PubSubEvent<Person>
    {
    }
}
