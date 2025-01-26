using Prism.Events;

namespace WpfPracticeDemo.ViewModels
{
    internal class ContentViewModel : DemoVmBase
    {
        public ContentViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        protected override void OnLoaded(object parameter)
        {

        }
    }
}
