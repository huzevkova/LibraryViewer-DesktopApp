
using System.ComponentModel;

namespace KniznicaWpfApp.UserControl
{
    /// <summary>
    /// Interaction logic for FormularKnihyUserControl.xaml
    /// </summary>
    public partial class InputPoleUserControl
    {
         private string _textPola = "";

         [Browsable(true), Category, Description]
         public string TextPola
         {
              get => _textPola;
              set
              {
                   _textPola = value;
                   PoleBlock.Text = _textPola;
              }
         }

         public string Input
         {
              get => InputBox.Text;
              set => InputBox.Text = value;
         }


          public InputPoleUserControl()
          {
             InitializeComponent();
             PoleBlock.Text = _textPola;
          }
    }
}
