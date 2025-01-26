using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace WpfPracticeDemo.Models
{
    internal class OperationShapeMenu : BindableBase
    {
        private bool _isExpanded;

        public string ShapeMenuName { get; set; }

        public ObservableCollection<string> ShapeTypes { get; } = new ObservableCollection<string>();

        public bool IsExpanded
        {
            get { return _isExpanded; }

            set { SetProperty(ref _isExpanded, value); }
        }
    }
}
