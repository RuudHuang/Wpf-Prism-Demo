using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Helpers
{
    internal class EventToCommandHelpercs
    {
        public static DemoEventTrigger GetEventTrigger(DependencyObject obj)
        {
            return (DemoEventTrigger)obj.GetValue(EventTriggerProperty);
        }

        public static void SetEventTrigger(DependencyObject obj, DemoEventTrigger value)
        {
            obj.SetValue(EventTriggerProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventTriggerProperty =
            DependencyProperty.RegisterAttached("EventTrigger", typeof(DemoEventTrigger), typeof(EventToCommandHelpercs), new PropertyMetadata(null, PropertyChangedHandler));


        private static void PropertyChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var eventTriggerInfo = GetEventTrigger(d);

            var eventName= eventTriggerInfo.EventName;   
            var targetCommand= eventTriggerInfo.TargetCommand;

            if (!string.IsNullOrEmpty(eventName) && targetCommand!=null)
            {
                var eventInfo = d.GetType().GetEvent(eventName);
                if (eventInfo != null)
                {
                    eventInfo.AddEventHandler(d, new RoutedEventHandler(RoutedEventHandler));
                }
            }
        }

        private static void RoutedEventHandler(object sender, RoutedEventArgs e)
        {
            var eventTriggerInfo = GetEventTrigger(sender as DependencyObject);
            var targetCommand= eventTriggerInfo.TargetCommand;
            if (targetCommand != null)
            {
                targetCommand.Execute(sender);
            }
        }

    }
}
