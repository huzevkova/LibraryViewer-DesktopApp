using System.Windows;

namespace KniznicaWpfApp
{
    /// <summary>
    /// Interaction logic for AboutWin.xaml
    /// </summary>
    public partial class AboutWin
    {
        public AboutWin()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
             Close();
        }
     }
}
