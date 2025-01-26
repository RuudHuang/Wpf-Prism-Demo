using Prism.Events;
using Prism.Ioc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Helpers;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;
using WpfPracticeDemo.ViewModels;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.Services
{
    internal class StartActionService : IStartActionService
    {

        private readonly IEventAggregator _eventAggregator;

        private readonly ObservableCollection<IStartAction> _startActions;

        private readonly IContainerProvider _containerProvider;

        private UcBootAdvancedActionView _bootAdvancedActionView;

        private double TotalPercentage
        {
            get
            {
                return _startActions.Sum(x => x.StartActionProgressPercentage);
            }
        }

        public ObservableCollection<IStartAction> StartActions => _startActions;

        public StartActionService(IEventAggregator eventAggregator,
            IContainerProvider containerProvider)
        {
            _startActions = new ObservableCollection<IStartAction>();
            _eventAggregator = eventAggregator;
            _containerProvider = containerProvider;
        }

        public Task<StartActionResult> ExcuteStartActions()
        {
            var startActionTask = Task.Run(() =>
            {
                StartActionResult result = default;

                foreach (var startAction in _startActions)
                {
                    UpdateProgress(startAction.ActionName, 0);

                    result = startAction.Excute();

                    UpdateProgress(startAction.ActionName, startAction.StartActionProgressPercentage);

                    if (result.Equals(StartActionResult.Failed))
                    {
                        break;
                    }
                }

                Thread.Sleep(500);

                return result;

            }); ;

            return startActionTask;
        }

        private void UpdateProgress(string actionName, double excutedPercentage)
        {
            Task.Run(() =>
             {
                 StartActionProgressChangedEventArgs startActionProgressChangedEventArgs = new StartActionProgressChangedEventArgs()
                 {
                     StartActionName = actionName,
                     PercentageInAllAction = excutedPercentage / TotalPercentage
                 };

                 _eventAggregator.GetEvent<StartActionProgressChangedEvent>().Publish(startActionProgressChangedEventArgs);

             }).Wait();
        }

        public void ShowBootAdvancedView()
        {
            if (_bootAdvancedActionView == null)
            {
                _bootAdvancedActionView = _containerProvider.Resolve<UcBootAdvancedActionView>();
                var bootAdvancedActionViewModel = _containerProvider.Resolve<BootAdvancedActionViewModel>();
                _bootAdvancedActionView.DataContext = bootAdvancedActionViewModel;
            }

            _bootAdvancedActionView.ShowDialog();
        }

        public void CloseBootAdvancedView()
        {
            ThreadHelper.ExcutedInUiThread(() =>
            {
                _bootAdvancedActionView.DialogResult = true;
            });
        }
    }
}
