using Deadfile.Model;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Deadfile.Events
{
    public class SelectedPersonChangeEvent : PubSubEvent<Person>
    {
    }
}
