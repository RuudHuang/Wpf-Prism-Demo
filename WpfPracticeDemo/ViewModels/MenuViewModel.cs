using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPracticeDemo.Commands;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.ViewModels
{
    internal class MenuViewModel:DemoVmBase
    {

        private OperationType _selectedOperationType;

        public ObservableCollection<OperationType> OperationTypes { get; set; }

        public OperationType SelectedOperationType
        { 
          get { return _selectedOperationType; }
            set 
            {
                if (SetProperty(ref _selectedOperationType, value))
                {
                    _eventAggregator.GetEvent<OperationTypeChangedEvent>().Publish(value);
                }
            }
        }

        public ICommand TestCommand { get; set; }

        private readonly IRegionManager _regionManager;

        public MenuViewModel(IRegionManager regionManager,IEventAggregator eventAggregator):base(eventAggregator)
        {
            _regionManager = regionManager;
            TestCommand = new DemoCommand(Navigate);

            OperationTypes = new ObservableCollection<OperationType>()
            {
               OperationType.DrawGraphic,
                OperationType.Select
            };
        }

        private void Navigate()
        {
            _regionManager.RequestNavigate(Constants.DemoRegionConstants.ContentRegionName, nameof(UcShapeOptionView));
        }

        protected override void OnLoaded(object parameter)
        {            
            SelectedOperationType = OperationType.DrawGraphic;
        }
    }
}
