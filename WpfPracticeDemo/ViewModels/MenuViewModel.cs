using Prism.Events;
using System.Collections.ObjectModel;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Helpers;
using WpfPracticeDemo.Interfaces;

namespace WpfPracticeDemo.ViewModels
{
    internal class MenuViewModel : DemoVmBase
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
                    var operationType = EnumHelper.GetEnum<OperationType>(value);
                    _operationTypeService.SetOperationType(operationType);
                }
            }
        }

        public MenuViewModel(IEventAggregator eventAggregator,
                             IOperationTypeService operationTypeService)
                       : base(eventAggregator)
        {
            _operationTypeService = operationTypeService;

            OperationTypes = new ObservableCollection<string>();

        }

        private void InitializeOperationType()
        {
            var operationTypes = EnumHelper.GetEnumDescriptions<OperationType>();

            OperationTypes.AddRange(operationTypes);
        }

        protected override void OnLoaded(object parameter)
        {
            InitializeOperationType();

            SelectedOperationType = OperationTypes[0];
        }        
    }
}
