using Prism.Events;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.ViewModels;

namespace WpfPracticeDemo
{
    public class MainWindowViewModel : DemoVmBase
    {

        private readonly IDemoRegionNavigateService _demoRegionNavigateService;

        public MainWindowViewModel(IDemoRegionNavigateService demoRegionNavigateService,
            IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _demoRegionNavigateService = demoRegionNavigateService;
        }

        private void NavigateToDefaultView()
        {
            _demoRegionNavigateService.NavigateAllRegionToDefaultView();
        }

        protected override void OnLoaded(object parameter)
        {
            NavigateToDefaultView();
        }
    }
}
