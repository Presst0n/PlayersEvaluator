using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using PE.WPF.EventModels;

namespace PE.WPF.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly MyRostersViewModel _myRostersVM;
        private readonly IEventAggregator _events;
        private readonly SimpleContainer _container;

        public ShellViewModel(MyRostersViewModel myRostersVM, IEventAggregator events,
            SimpleContainer container)
        {
            _events = events;
            _myRostersVM = myRostersVM;
            _container = container;

            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async void DisplayRegisterView()
        {
            await ActivateItemAsync(_container.GetInstance<RegisterViewModel>());
        }

        public async void DisplayLoginView()
        {
            await ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_myRostersVM);
        }
    }
}
