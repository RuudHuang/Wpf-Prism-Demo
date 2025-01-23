using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPracticeDemo.Commands;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Services;
using WpfPracticeDemo.ViewModels;

namespace WpfPracticeDemo
{
    public class MainWindowViewModel: DemoVmBase
    {

        private readonly IDemoRegionNavigateService _demoRegionNavigateService;                

        public MainWindowViewModel(IDemoRegionNavigateService demoRegionNavigateService,
            IEventAggregator eventAggregator):base(eventAggregator)
        {
            _demoRegionNavigateService= demoRegionNavigateService;            
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
