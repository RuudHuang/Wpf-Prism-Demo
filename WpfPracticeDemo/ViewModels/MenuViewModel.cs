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
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Views;

namespace WpfPracticeDemo.ViewModels
{
    internal class MenuViewModel:DemoVmBase
    {

        private OperationType _selectedOperationType;

        private readonly IOperationTypeService _operationTypeService;

        public ObservableCollection<OperationType> OperationTypes { get; set; }

        public OperationType SelectedOperationType
        { 
          get { return _selectedOperationType; }
            set 
            {
                if (SetProperty(ref _selectedOperationType, value))
                {
                    _operationTypeService.SetOperationType(value);
                }
            }
        }

        public MenuViewModel(IEventAggregator eventAggregator,
                             IOperationTypeService operationTypeService)
                       :base(eventAggregator)
        {
            _operationTypeService = operationTypeService;            

            OperationTypes = new ObservableCollection<OperationType>()
            {
                OperationType.DrawGraphic,
                OperationType.Select
            };
        }

        protected override void OnLoaded(object parameter)
        {            
            SelectedOperationType = OperationType.DrawGraphic;
        }
    }
}
