using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPracticeDemo.Models
{
    internal class OperationShapeMenu:BindableBase
    {
        private bool _isExpanded;

        public string ShapeMenuName {  get; set; }

        public ObservableCollection<ShapeBase> Shapes { get;} = new ObservableCollection<ShapeBase>();

        public bool IsExpanded
        {
            get { return _isExpanded; }

            set { SetProperty(ref _isExpanded, value); }
        }
    }
}
