using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfPracticeDemo.Models
{
    internal class DemoEventTrigger:Freezable
    {
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EventName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(DemoEventTrigger), new PropertyMetadata(string.Empty));

        public ICommand TargetCommand
        {
            get { return (ICommand)GetValue(TargetCommandProperty); }
            set { SetValue(TargetCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetCommandProperty =
            DependencyProperty.Register("TargetCommand", typeof(ICommand), typeof(DemoEventTrigger), new PropertyMetadata(null));


        protected override Freezable CreateInstanceCore()
        {
            return new DemoEventTrigger();
        }
    }
}
