using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace WpfPracticeDemo.Models
{
    internal class OperationShapeMenu : BindableBase
    {
        private bool _isEnable = false;

        private bool _isExpanded;

        public string ShapeMenuName { get; set; }

        public ObservableCollection<ShapeTypeDisplayModel> ShapeTypes { get; } = new ObservableCollection<ShapeTypeDisplayModel>();

        public bool IsExpanded
        {
            get { return _isExpanded; }

            set { SetProperty(ref _isExpanded, value); }
        }

        public bool IsEnable
        { 
          get => _isEnable;

            set
            { 
              SetProperty(ref _isEnable, value);
            }
        }
    }
}
