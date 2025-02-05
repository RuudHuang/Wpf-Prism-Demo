using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfPracticeDemo.Adorners;

namespace WpfPracticeDemo.Views
{
    /// <summary>
    /// Interaction logic for UcBootAdvancedActionView.xaml
    /// </summary>
    public partial class UcBootAdvancedActionView : Window
    {
        public UcBootAdvancedActionView()
        {            
            InitializeComponent();

            this.Loaded += UcBootAdvancedActionView_Loaded;
        }

        private void UcBootAdvancedActionView_Loaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this.MainContentContainer)?.Add(new CommonAdorner(this.MainContentContainer, new AdornerLoadingView()));
        }
    }
}
