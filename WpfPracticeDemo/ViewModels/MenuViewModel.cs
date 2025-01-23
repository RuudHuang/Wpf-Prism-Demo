using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPracticeDemo.Commands;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.ViewModels
{
    internal class MenuViewModel
    {
        public ICommand TestCommand { get; set; }

        private readonly IRegionManager _regionManager;

        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            TestCommand = new DemoCommand(Navigate);
        }

        private void Navigate()
        {
            _regionManager.RequestNavigate(Constants.DemoRegionConstants.ContentRegionName, nameof(UcOperationView));
        }
    }
}
