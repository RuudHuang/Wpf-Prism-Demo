using Prism;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Unity;
using System.Windows;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Services;
using WpfPracticeDemo.StartActions;
using WpfPracticeDemo.ViewModels;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.BootStrapper
{
    internal class DemoBootStrapper : PrismBootstrapper
    {
        protected override void Initialize()
        {
            base.Initialize();

            var startActionService = Container.Resolve<IStartActionService>();

            startActionService.StartActions.Add(new StartActionInitializeData());

            startActionService.ShowBootAdvancedView();

        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<UcMenuView, MenuViewModel>();
            containerRegistry.RegisterForNavigation<UcShapeOptionView, ShapeOptionViewModel>();
            containerRegistry.RegisterForNavigation<UcContentView, ContentViewModel>();
            containerRegistry.RegisterSingleton<IDemoRegionNavigateService, DemoRegionNavigateService>();              
            containerRegistry.RegisterSingleton<IStartActionService, StartActionService>();
            containerRegistry.RegisterSingleton<IGeometryService, GeometryService>();

            containerRegistry.RegisterSingleton<IOperationTypeService, OperationTypeService>();
            containerRegistry.RegisterSingleton<IDrawingShapeTypeService, DrawingShapeTypeService>();
        }

        private void InitializeStartActions()
        {
                       
        }

        public static void ShutDownApplication()
        {
            Application.Current.Shutdown();
        }        
        
    }
}
