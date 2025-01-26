using Prism.Events;
using Prism.Mvvm;
using System.Windows.Input;
using WpfPracticeDemo.Commands;

namespace WpfPracticeDemo.ViewModels
{
    public abstract class DemoVmBase : BindableBase
    {
        public ICommand LoadedCommand { get; private set; }

        protected readonly IEventAggregator _eventAggregator;

        public DemoVmBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            Initialize();
        }

        private void Initialize()
        {
            LoadedCommand = new DemoCommand(OnLoaded);
        }

        protected abstract void OnLoaded(object parameter);

    }
}
