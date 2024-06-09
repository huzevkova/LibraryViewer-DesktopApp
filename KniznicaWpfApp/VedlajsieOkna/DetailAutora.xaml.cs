using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using KniznicaLibrary;

namespace KniznicaWpfApp
{
     /// <summary>
     /// Interaction logic for DetailAutora.xaml
     /// </summary>
     public partial class DetailAutora
     {
          private readonly Autor _autor;
          public DetailAutora(Autor autor)
          {
               InitializeComponent();
               _autor = autor;
               DataContext = _autor;


               if (autor.Obrazok != "")
               {
                    image.ImageSource = new BitmapImage(new Uri($"Resources/AutoriObrazky/{autor.Obrazok}", UriKind.Relative));
               }
               else
               {
                    image.ImageSource = new BitmapImage(new Uri("Resources/AutoriObrazky/defaultA.png", UriKind.Relative));
               }

               if (autor.DatumUmrtia == "")
               {
                    roky.Text = autor.DatumNarodenia;
               }
               else
               {
                    roky.Text = autor.DatumNarodenia + " - " + autor.DatumUmrtia;
               }

               foreach (var kniha in autor.ZoznamKnihAutora)
               {
                    TextBlock nazov = new()
                    {
                         Text = kniha.Nazov,
                         HorizontalAlignment = HorizontalAlignment.Left,
                         FontWeight = FontWeights.Bold,
                         Margin = new Thickness(10)
                    };

                    TextBlock rok = new()
                    {
                         Text = kniha.RokVydania.ToString(),
                         HorizontalAlignment = HorizontalAlignment.Right,
                         Margin = new Thickness(10)
                    };

                    Grid riadok = new();
                    riadok.Children.Add(nazov);
                    riadok.Children.Add(rok);

                    knihyPanel.Children.Add(riadok);
               }

               if (_autor.Kod != null)
               {
                    meno.Cursor = Cursors.Hand;
               }
          }

          private void ZmenaVelkosti(object sender, SizeChangedEventArgs e)
          {
               var size = (int)e.NewSize.Height;

               elipsa.Width = (double)size * 2 / 5; 
               elipsa.Height = (double)size * 2 / 5;
          }

          private void OtvorStranku(object sender, System.Windows.Input.MouseButtonEventArgs e)
          {
               if (_autor.Kod != null)
               {
                    string source = "https://openlibrary.org/authors/" + _autor.Kod + "/" +
                                    _autor.Meno.Replace(" ", "_");
                    try
                    {
                         Process.Start(new ProcessStartInfo
                         {
                              FileName = source,
                              UseShellExecute = true
                         });
                    }
                    catch (Exception ex)
                    {
                         Console.WriteLine("An error occurred: " + ex.Message);
                    }

               }
          }
     }
}
