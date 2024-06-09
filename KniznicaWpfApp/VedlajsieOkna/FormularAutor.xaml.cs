using System.Windows;
using KniznicaLibrary;

namespace KniznicaWpfApp
{
     /// <summary>
     /// Interaction logic for FormularAutor.xaml
     /// </summary>
     public partial class FormularAutor
     {

          public string Meno
          {
               get => meno.Input;
               set => meno.Input = value;
          }

          public string? Narodenie
          {
               get => narodenie.SelectedDate?.ToString("dd.MM.yyyy");
               set
               {
                    if (value != null)
                    {
                         narodenie.SelectedDate = DateTime.Parse(value);
                    }
               }
          }

          public string? Umrtie
          {
               get => umrtie.SelectedDate?.ToString("dd.MM.yyyy");
               set
               {
                    if (value != null)
                    {
                         umrtie.SelectedDate = DateTime.Parse(value);
                    }
               }
          }

          public string? Obrazok => subor.File;

          public FormularAutor(Autor? autor = null)
          {
               InitializeComponent();
               if (autor != null)
               {
                    Meno = autor.Meno;
                    Narodenie = autor.DatumNarodenia;
                    Umrtie = autor.DatumUmrtia;
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
