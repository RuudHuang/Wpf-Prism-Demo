using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfPracticeDemo.BootStrapper;
using WpfPracticeDemo.Commands;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Helpers;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.ViewModels
{
    internal class BootAdvancedActionViewModel:BindableBase
    {

        private readonly IEventAggregator _eventAggregator;

        private readonly IStartActionService _startActionService;

        private string _excutingActionName=string.Empty;

        public string ExcutingActionName
        { 
           get { return _excutingActionName; }

            set 
            {
                SetProperty(ref _excutingActionName, value);
            }
        }

        private double _progressValue;

        public double ProgressValue
        {
            get { return _progressValue; }
            set 
            {
                SetProperty(ref _progressValue, value);
            }
        }

        public ICommand ExcuteStartActionsCommand { get; set; }

        public BootAdvancedActionViewModel(IEventAggregator eventAggregator,
            IStartActionService startActionService)
        {
            _eventAggregator= eventAggregator;
            _startActionService= startActionService;

            ExcuteStartActionsCommand = new DemoCommand(ExcuteStartActions);

            ManageSubscribe(true);
        }


        private void ManageSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                _eventAggregator.GetEvent<StartActionProgressChangedEvent>().Subscribe(UpdateProgress);
            }
            else
            {
                _eventAggregator.GetEvent<StartActionProgressChangedEvent>().Unsubscribe(UpdateProgress);
            }
        }

        private void UpdateProgress(StartActionProgressChangedEventArgs args)
        {
            ThreadHelper.ExcutedInUiThread(() =>
            {
                ExcutingActionName = args.StartActionName;
                ProgressValue += args.PercentageInAllAction*100;
            });
        }

        private async void ExcuteStartActions()
        {
            var startActionResult=await _startActionService.ExcuteStartActions();

            if (startActionResult.Equals(StartActionResult.Failed))
            {
                DemoBootStrapper.ShutDownApplication();
            }
            else
            {
                _startActionService.CloseBootAdvancedView();
            }
        }

        ~BootAdvancedActionViewModel()
        {
            ManageSubscribe(false);
        }
    }
}
