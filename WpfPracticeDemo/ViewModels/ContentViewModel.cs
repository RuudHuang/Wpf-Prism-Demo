using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPracticeDemo.ViewModels
{
    internal class ContentViewModel : DemoVmBase
    {
        public ContentViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        protected override void OnLoaded(object parameter)
        {
            
        }
    }
}
