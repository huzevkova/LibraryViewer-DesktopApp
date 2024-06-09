using System.Windows;
using KniznicaLibrary;

namespace KniznicaWpfApp
{
    /// <summary>
    /// Interaction logic for FormularKnihy.xaml
    /// </summary>
    public partial class FormularKnihy
    {
         public string Nazov
         {
              get => nazov.Input;
              set => nazov.Input = value;
         }

         public string Autor
         {
              get => autor.Text;
              set => autor.Text = value;
         }

         public string Rok
         {
              get => rok.Input;
              set => rok.Input = value;
         }

         public string Vydavatelstvo
         {
              get => vydavatelstvo.Input;
              set => vydavatelstvo.Input = value;
         }

         public string Popis
         {
              get => popis.Input;
              set => popis.Input = value;
         }

         public string Poznamky
         {
              get => poznamky.Input;
              set => poznamky.Input = value;
         }

         public int Hodnotenie
         {
              get => (int)hodnotenieSlider.Value;
              set => hodnotenieSlider.Value = value;
         }

         public bool Precitana
         {
              get => precitana.IsChecked == true;
              set => precitana.IsChecked = value;
         }

         public bool NaNeskor
         {
              get => naNeskor.IsChecked == true;
              set => naNeskor.IsChecked = value;
         }

         public bool Kupena
         {
              get => kupena.IsChecked == true;
              set => kupena.IsChecked = value;
         }

         public bool Pozicana
         {
              get => pozicana.IsChecked == true;
              set => pozicana.IsChecked = value;
         }

         public string? Obrazok
         {
              get => pridajSubor.File;
          }

         public FormularKnihy(Kniha? kniha = null, ZoznamAutorov? autori = null)
         {
            InitializeComponent();
            if (kniha != null)
            {
                 Nazov = kniha.Nazov;
                 Autor = kniha.AutorKnihy;
                 Rok = kniha.RokVydania.ToString() ?? "";
                 Vydavatelstvo = kniha.Vydavatelstvo;
                 Hodnotenie = (int)kniha.Hodnotenie;
                 Popis = kniha.Popis;
                 Poznamky = kniha.Poznamky;
                 Pozicana = kniha.Pozicana;
                 Kupena = kniha.Kupena;
                 Precitana = kniha.Precitana;
                 NaNeskor = kniha.NaNeskor;
            }

            if (autori != null)
            {
                 autor.ItemsSource = autori.DajLenMena();
            }
         } 

         private void Zrus(object sender, RoutedEventArgs e)
         {
             Close();
         }

         private void Hotovo(object sender, RoutedEventArgs e)
         {
              DialogResult = true;
         }
    }
}
