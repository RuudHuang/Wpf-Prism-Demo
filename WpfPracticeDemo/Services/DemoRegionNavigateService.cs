using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.Services
{
    internal class DemoRegionNavigateService: IDemoRegionNavigateService
    {
        private IRegionManager _regionManager;

        public DemoRegionNavigateService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void NavigateAllRegionToDefaultView()
        {
            _regionManager.RequestNavigate(Constants.DemoConstants.MenuRegionName, nameof(UcMenuView));
            _regionManager.RequestNavigate(Constants.DemoConstants.OperationRegionName, nameof(UcOperationView));
            _regionManager.RequestNavigate(Constants.DemoConstants.ContentRegionName, nameof(UcContentView));
        }

        public void NavigateToSpecificView<T>()
        {
            _regionManager.RequestNavigate(Constants.DemoConstants.ContentRegionName, nameof(T));
        }
    }
}
