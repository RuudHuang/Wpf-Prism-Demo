﻿using Prism.Regions;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.Services
{
    internal class DemoRegionNavigateService : IDemoRegionNavigateService
    {
        private IRegionManager _regionManager;

        public DemoRegionNavigateService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void NavigateAllRegionToDefaultView()
        {
            _regionManager.RequestNavigate(Constants.DemoRegionConstants.MenuRegionName, nameof(UcMenuView));
            _regionManager.RequestNavigate(Constants.DemoRegionConstants.ShapeOptionRegionName, nameof(UcShapeOptionView));
            _regionManager.RequestNavigate(Constants.DemoRegionConstants.ContentRegionName, nameof(UcContentView));
        }

        public void NavigateToSpecificView<T>()
        {
            _regionManager.RequestNavigate(Constants.DemoRegionConstants.ContentRegionName, nameof(T));
        }
    }
}
