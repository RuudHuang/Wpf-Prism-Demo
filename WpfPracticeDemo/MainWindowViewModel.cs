using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPracticeDemo.Commands;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Services;

namespace WpfPracticeDemo
{
    public class MainWindowViewModel
    {

        private readonly IDemoRegionNavigateService _demoRegionNavigateService;

        public ICommand NavigateToDefaultViewCommand { get; set; }

        public MainWindowViewModel(IDemoRegionNavigateService demoRegionNavigateService)
        {
            _demoRegionNavigateService= demoRegionNavigateService;
            NavigateToDefaultViewCommand = new DemoCommand(NavigateToDefaultView);
        }

        private void NavigateToDefaultView()
        {
            _demoRegionNavigateService.NavigateAllRegionToDefaultView();
        }
    }
}
