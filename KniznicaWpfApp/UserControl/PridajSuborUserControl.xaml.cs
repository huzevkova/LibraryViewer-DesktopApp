using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace KniznicaWpfApp
{
     /// <summary>
     /// Interaction logic for PridajSuborUserControl.xaml
     /// </summary>
     public partial class PridajSuborUserControl
     {

          public string? TextTlacidla
          {
               get => pridajButton.Content.ToString();
               set => pridajButton.Content = value;
          }

          private string _filter = "All files(*.*)|*.*";

          public string Filter
          {
               set => _filter = value;
          }

          private string? _file;

          public string? File
          {
               get => _file;
          }

          public PridajSuborUserControl()
          {
               InitializeComponent();
          }

          private void OtvorSubory(object sender, RoutedEventArgs e)
          {
               var openFileDialog = new OpenFileDialog
               {
                    Filter = _filter
               };

               var result = openFileDialog.ShowDialog();

               if (result != true) return;
               _file = openFileDialog.FileName;
               subor.Content = Path.GetFileName(_file);
          }
     }
}
