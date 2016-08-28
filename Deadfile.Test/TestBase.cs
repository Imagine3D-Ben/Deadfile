using Microsoft.Practices.Prism.PubSubEvents;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Deadfile.Data;
using Deadfile.Events;
using Deadfile.Helpers;
using Deadfile.Model;
using Deadfile.Services;
using ObservableImmutable;
using System.Windows.Threading;
using System.Threading;

namespace Deadfile.Test
{
    public static class IntegrationTestsSynchronization
    {
        public static readonly object LockObject = new object();
    }
    public class TestBase
    {
        protected Mock<IPersonService> personServiceMock = new Mock<IPersonService>();
        protected Mock<IDispatcher> dispatcherMock = new Mock<IDispatcher>();
        protected Mock<IEventAggregator> aggregatorMock = new Mock<IEventAggregator>();
        protected Mock<IDialogService> dialogServiceMock = new Mock<IDialogService>();
        protected Mock<IChubbFactory> chubbFactoryMock = new Mock<IChubbFactory>();
        protected Mock<ITaskScheduler> taskSchedulerMock = new Mock<ITaskScheduler>();
        protected Mock<IChubb> chubMock = new Mock<IChubb>();

        protected Mock<SelectedPersonChangeEvent> currentPersonChangeEventMock = new Mock<SelectedPersonChangeEvent>();
        protected Mock<PersonDirectoryUpdatedEvent> personDirectoryUpdatedEventMock = new Mock<PersonDirectoryUpdatedEvent>();
        protected Mock<PersonDeletedEvent> personDeletedEventMock = new Mock<PersonDeletedEvent>();
        protected Mock<PersonFilterEvent> personFilterEventMock = new Mock<PersonFilterEvent>();

        protected readonly List<Person> persons;

        public TestBase()
        {
            persons = new List<Person>()
            {
                new Person() { Id = 1, Age = 20, FirstName = "Name 1", LastName = "Surname 1" },
                new Person() { Id = 2, Age = 30, FirstName = "Name 2", LastName = "Surname 2"},
                new Person() { Id = 3, Age = 40, FirstName = "Name 3", LastName = "Surname 3"}
            };
        }

        protected void TestSetup()
        {
            Monitor.Enter(IntegrationTestsSynchronization.LockObject);

            currentPersonChangeEventMock = new Mock<SelectedPersonChangeEvent>();
            personDirectoryUpdatedEventMock = new Mock<PersonDirectoryUpdatedEvent>();
            personDeletedEventMock = new Mock<PersonDeletedEvent>();

            PersonServiceSetup();
            DispatcherSetup();
            AggregatorSetup();
            ChubFactorySetup();
            TaskSchedulerSetup();
        }

        public void TestTeardown()
        {
            Monitor.Exit(IntegrationTestsSynchronization.LockObject);
        }
        private void PersonServiceSetup()
        {
            personServiceMock.Setup(x => x.GetPersons()).Returns(() => persons);
        }

        private void DispatcherSetup()
        {
            dispatcherMock.Setup(x => x.Invoke(It.IsAny<Action>())).Callback((Action a) => a());
            dispatcherMock.Setup(x => x.BeginInvoke(It.IsAny<Action>())).Callback((Action a) => a());
            dispatcherMock.Setup(x => x.BeginInvoke(It.IsAny<DispatcherPriority>(), It.IsAny<Action>())).Callback((DispatcherPriority priority, Action a) => a());
            dispatcherMock.Setup(x => x.BeginInvoke(It.IsAny<DispatcherPriority>(), It.IsAny<DispatcherOperationCallback>(), It.IsAny<DispatcherFrame>())).Callback((DispatcherPriority priority, DispatcherOperationCallback callback, DispatcherFrame frame) => callback.Invoke(frame));
            dispatcherMock.Setup(x => x.PushFrame(It.IsAny<DispatcherFrame>())).Callback((DispatcherFrame frame) => { });
            dispatcherMock.Setup(x => x.BackgroundThread()).Returns(ThreadOption.UIThread);
            dispatcherMock.Setup(x => x.UIThread()).Returns(ThreadOption.UIThread);
            dispatcherMock.Setup(x => x.Invoke(It.IsAny<DispatcherPriority>(), It.IsAny<Action<object, object>>(), It.IsAny<object>(), It.IsAny<object>())).Callback((DispatcherPriority priority, Action<object, object> del, object invoker, object args) => del(invoker, args));
        }

        private void ChubFactorySetup()
        {
            chubbFactoryMock.Setup(x => x.CreateChubb()).Returns(chubMock.Object);
            chubMock.Setup(x => x.Lock(It.IsAny<LockTypeEnum>())).Callback((LockTypeEnum a) => { });
            chubMock.Setup(x => x.Unlock(It.IsAny<LockTypeEnum>())).Callback((LockTypeEnum a) => { });
            chubMock.Setup(x => x.TryLock(It.IsAny<LockTypeEnum>())).Returns(true);
            chubMock.Setup(x => x.WaitForCondition(It.IsAny<LockTypeEnum>(), It.IsAny<Func<bool>>())).Callback((LockTypeEnum l, Func<bool> condition) => { });
        }

        private void TaskSchedulerSetup()
        {
            taskSchedulerMock.Setup(x => x.Run(It.IsAny<Action>())).Returns((Action a) => Task.Run(a));
        }

        private void AggregatorSetup()
        {
            aggregatorMock.Setup(x => x.GetEvent<SelectedPersonChangeEvent>()).Returns(currentPersonChangeEventMock.Object);
            aggregatorMock.Setup(x => x.GetEvent<PersonDirectoryUpdatedEvent>()).Returns(personDirectoryUpdatedEventMock.Object);
            aggregatorMock.Setup(x => x.GetEvent<PersonDeletedEvent>()).Returns(personDeletedEventMock.Object);
            aggregatorMock.Setup(x => x.GetEvent<PersonFilterEvent>()).Returns(personFilterEventMock.Object);
        }

        protected void AssertValidMessageBoxWasDisplayed(object expectedViewModel, string expectedText, string expectedCaption, MessageBoxButton expectedButtons, MessageBoxImage expectedImage, string failMessage)
        {
            dialogServiceMock.Verify(x => x.ShowMessageBox(
                It.Is<object>(o => object.ReferenceEquals(o, expectedViewModel)),
                It.Is<string>(s => s.Equals(expectedText)),
                It.Is<string>(s => s.Equals(expectedCaption)),
                It.Is<MessageBoxButton>(b => b.Equals(expectedButtons)),
                It.Is<MessageBoxImage>(i => i.Equals(expectedImage))), Times.Once, failMessage);
        }

        protected void AssertMessageBoxWasDisplayed(string failMessage)
        {
            dialogServiceMock.Verify(x => x.ShowMessageBox(
                It.IsAny<object>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()), Times.Once, failMessage);
        }
    }
}
