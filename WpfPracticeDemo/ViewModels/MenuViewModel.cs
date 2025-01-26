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
        private string _selectedOperationType;

        private readonly IOperationTypeService _operationTypeService;

        public ObservableCollection<string> OperationTypes { get; set; }

        public string SelectedOperationType
        { 
          get { return _selectedOperationType; }
            set 
            {
                if (SetProperty(ref _selectedOperationType, value))
                {
                    var operationType = GetOperationType(value);
                    _operationTypeService.SetOperationType(operationType);
                }
            }
        }

        public MenuViewModel(IEventAggregator eventAggregator,
                             IOperationTypeService operationTypeService)
                       :base(eventAggregator)
        {
            _operationTypeService = operationTypeService;            

            OperationTypes = new ObservableCollection<string>()
            {
                nameof(OperationType.DrawGraphic),
                nameof(OperationType.Select)
            };
        }

        protected override void OnLoaded(object parameter)
        {            
            SelectedOperationType = OperationTypes[0];
        }

        private OperationType GetOperationType(string operationTypeName)
        {
            switch (operationTypeName)
            { 
               case nameof(OperationType.Select):
                    return OperationType.Select;
                    case nameof(OperationType.DrawGraphic): 
                    return OperationType.DrawGraphic;
                default:
                    return OperationType.DrawGraphic;
            }
        }
    }
}
