using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using WpfPracticeDemo.Adorners;

namespace WpfPracticeDemo.Helpers
{
    internal class AdornerHelper
    {
        public static bool GetHasloadingAdnorner(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasloadingAdnornerProperty);
        }

        public static void SetHasloadingAdnorner(DependencyObject obj, bool value)
        {
            obj.SetValue(HasloadingAdnornerProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasloadingAdnornerProperty =
            DependencyProperty.RegisterAttached("HasloadingAdnorner", typeof(bool), typeof(AdornerHelper), new PropertyMetadata(false, AdornerChangedHandler));

        private static void AdornerChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hasLoadingAdnorner = GetHasloadingAdnorner(d);
            if (hasLoadingAdnorner)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(d as UIElement);
                if (adornerLayer != null)
                {
                    //adornerLayer.Add(new LoadingAdorner(d as UIElement));
                }
                
            }
        }


    }
}
