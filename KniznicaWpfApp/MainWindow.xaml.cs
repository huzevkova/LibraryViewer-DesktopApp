
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KniznicaLibrary;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using Path = System.IO.Path;

namespace KniznicaWpfApp
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow
     {
          private ZoznamKnih _aktualZoznamKnih = new("");
          private Kniha? _aktualKniha;
          private Autor? _aktualAutor;
          private readonly Kniznica _kniznica;
          private readonly ZoznamAutorov _zoznamAutorov;
          private bool _zmena;
          private List<string> _noveKnihy = [];
          private List<string> _noviAutori = [];
          private readonly Statistiky _stats;
          private readonly LinearGradientBrush _hlavnyBrush;
          private readonly RadialGradientBrush _vedlajsiBrush;

          public MainWindow()
          {
               InitializeComponent();
               var s = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent;
               if (s != null)
               {
                    Directory.SetCurrentDirectory(s.ToString());
               }


               WindowState = WindowState.Maximized;
               Closing += MainWindow_Closing;
               DataContext = this;

               _kniznica = new Kniznica(new FileInfo(StringKonst.KnihySubor), new FileInfo(StringKonst.AutoriSubor));

               foreach (var zoznam in _kniznica.KniznicaList)
               {
                    Button button = new()
                    {
                         Content = zoznam.NazovZoznamu,
                         Margin = new Thickness(5),
                         FontSize = 16
                    };
                    button.Click += OtvorZoznam;
                    PolickyStack.Children.Add(button);
               }

               _zoznamAutorov = _kniznica.ZoznamVsetkychAutorov;
               KnihyList.ItemsSource = _aktualZoznamKnih;
               AutoriList.ItemsSource = _zoznamAutorov;

               //inšpirácia na prácu s gradientami bola zo stránky:
               //https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/painting-with-solid-colors-and-gradients-overview?view=netframeworkdesktop-4.8

               _hlavnyBrush = new LinearGradientBrush
               {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 1)
               };
               _hlavnyBrush.GradientStops.Add(new GradientStop(Colors.Gold, 0.0));
               _hlavnyBrush.GradientStops.Add(new GradientStop(Colors.Brown, 0.25));
               _hlavnyBrush.GradientStops.Add(new GradientStop(Colors.Brown, 0.75));
               _hlavnyBrush.GradientStops.Add(new GradientStop(Colors.Gold, 1.0));
               mainGrid.Background = _hlavnyBrush;

               _vedlajsiBrush = new RadialGradientBrush
               {
                    GradientOrigin = new Point(0.5, 0.5),
                    Center = new Point(0.5, 0.5)
               };
               _vedlajsiBrush.GradientStops.Add(new GradientStop(Colors.FloralWhite, 0.0));
               _vedlajsiBrush.GradientStops.Add(new GradientStop(Colors.LightSkyBlue, 0.25));
               _vedlajsiBrush.GradientStops.Add(new GradientStop(Colors.FloralWhite, 0.75));
               _vedlajsiBrush.GradientStops.Add(new GradientStop(Colors.LightSkyBlue, 1.0));
               KnihyList.Background = _vedlajsiBrush;
               AutoriList.Background = _vedlajsiBrush;

               _stats = new Statistiky(_kniznica);
               PomocneFunkcie.NastavStatistiku(ref statistika, _stats);
          }


          private void OtvorZoznam(object sender, RoutedEventArgs e)
          {
               AutoriList.Visibility = Visibility.Hidden;
               KnihyList.Visibility = Visibility.Visible;
               string nazov = (string)((Button)e.Source).Content;
               _aktualZoznamKnih = _kniznica.KniznicaList.Find((knih => knih.NazovZoznamu == nazov)) ?? _kniznica.ZoznamVsetkychKnih;
               _stats.Zoznam = _aktualZoznamKnih;
               PomocneFunkcie.NastavStatistiku(ref statistika, _stats);
               KnihyList.SelectedItem = null;
               ImagePreview.Source = null;
               KnihyList.ItemsSource = _aktualZoznamKnih;
               NazovZoznamu.Content = _aktualZoznamKnih.NazovZoznamu.ToUpper();
          }

          private void VyberKnihy(object sender, SelectionChangedEventArgs e)
          {
               var item = KnihyList.SelectedItem;
               if (item != null)
               {
                    _aktualKniha = (Kniha)item;
                    BitmapImage image = new();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = PomocneFunkcie.DajObrazokUri(_aktualKniha.Obrazok, StringKonst.ObrazkyZdrojKnihy, StringKonst.KnihyDefault);
                    try
                    {
                         image.EndInit();
                         ImagePreview.Source = image;
                    } catch (Exception)
                    {
                         ImagePreview.Source = null;
                    }
               }
          }

          private void UkazAutorov(object sender, RoutedEventArgs e)
          {
               KnihyList.SelectedItem = null;
               AutoriList.Visibility = Visibility.Visible;
               KnihyList.Visibility = Visibility.Hidden;
               NazovZoznamu.Content = StringKonst.AutoriNadpis;
          }

          private void PridajPolicku(object sender, RoutedEventArgs e)
          {
               var result = Interaction.InputBox("Zadajte názov poličky:", "Nová polička");
               if (result == "")
               {
                    return;
               }
               _kniznica.KniznicaList.Add(new ZoznamKnih(result));
               Button button = new()
               {
                    Content = result,
                    Margin = new Thickness(5),
                    FontSize = 16
               };
               button.Click += OtvorZoznam;
               PolickyStack.Children.Add(button);
               _zmena = true;
          }

          private void PridajKnihu(object sender, RoutedEventArgs e)
          {
               FormularKnihy formular = new(null, _zoznamAutorov);
               var result = formular.ShowDialog() ?? false;

               if (result)
               {
                    int? rok = formular.Rok == "" ? null : int.Parse(formular.Rok);
                    var kniha = new Kniha(formular.Nazov, formular.Autor, rok,
                         formular.Vydavatelstvo, Path.GetFileName(formular.Obrazok) ?? "", formular.Popis,
                         formular.Poznamky, formular.Hodnotenie, formular.Precitana, formular.NaNeskor, formular.Kupena,
                         formular.Pozicana);

                    var filename = formular.Obrazok;
                    string destinationDirectory = StringKonst.ObrazkyZdrojKnihy;

                    if (File.Exists(filename))
                    { 
                         File.Copy(filename, destinationDirectory + Path.GetFileName(filename));
                         _noveKnihy.Add(Path.GetFileName(filename));
                    }

                    _kniznica.PridajKnihu(kniha, _aktualZoznamKnih.NazovZoznamu);
                    _zmena = true;
               }
          }

          private void OdstranObjekt(object sender, RoutedEventArgs e)
          {
               if (KnihyList.SelectedItem != null)
               {
                    var kniha = (Kniha)KnihyList.SelectedItem;

                    MessageBoxResult result;
                    if (_aktualZoznamKnih.NazovZoznamu != "Všetky knihy")
                    {
                         result = MessageBox.Show("Chcete odstrániť knihu aj zo všetkých ostatných zoznamov?",
                              "Odstránenie knihy", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                         if (result == MessageBoxResult.Cancel)
                         {
                              return;
                         }
                    }
                    else
                    {
                         result = MessageBox.Show("Chcete naozaj odstrániť knihu zo všetkých zoznamov?",
                              "Odstránenie knihy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                         if (result == MessageBoxResult.No)
                         {
                              return;
                         }
                    }


                    if (result == MessageBoxResult.Yes)
                    {
                         _kniznica.OdoberKnihu(kniha);
                    }
                    else
                    {
                         _aktualZoznamKnih.Remove(kniha);
                         KnihyList.ItemsSource = _aktualZoznamKnih;
                    }

                    ImagePreview.Source = null;
                    if (kniha.Obrazok != "")
                    {
                         var zdroj = $"{StringKonst.ObrazkyZdrojKnihy}{kniha.Obrazok}";
                         var ciel = $"{StringKonst.ObrazkyKnihyKos}{kniha.Obrazok}";
                         File.Move(zdroj, ciel);
                    }

                    _zmena = true;
               }
               else if (AutoriList.SelectedItem != null)
               {
                    var autor = (Autor)AutoriList.SelectedItem;
                    var result = MessageBox.Show("Chcete odstrániť aj všetky knihy autora?",
                         "Odstránenie autora", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Cancel)
                    {
                         return;
                    }

                    if (result == MessageBoxResult.Yes)
                    {
                         _kniznica.OdoberKnihy((kniha => kniha.AutorKnihy == autor.Meno));
                    }

                    ImagePreview.Source = null;
                    if (autor.Obrazok != "")
                    {
                         var zdroj = $"{StringKonst.ObrazkyZdrojAutori}{autor.Obrazok}";
                         var ciel = $"{StringKonst.ObrazkyAutoriKos}{autor.Obrazok}";
                         File.Move(zdroj, ciel);
                    }

                    _zoznamAutorov.Remove(autor);
                    AutoriList.ItemsSource = _zoznamAutorov;
                    _zmena = true;
               }
               PomocneFunkcie.NastavStatistiku(ref statistika, _stats);
          }

          private void VyberAutora(object sender, SelectionChangedEventArgs e)
          {
               var item = AutoriList.SelectedItem;
               if (item != null)
               {
                    _aktualAutor = (Autor)item;
                    BitmapImage image = new();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = PomocneFunkcie.DajObrazokUri(_aktualAutor.Obrazok, StringKonst.ObrazkyZdrojAutori, StringKonst.AutoriDeafult);

                    try
                    {
                         image.EndInit();
                         ImagePreview.Source = image;
                    }
                    catch (Exception)
                    {
                         ImagePreview.Source = null;
                    }
               }
          }

          private void OPrograme(object sender, RoutedEventArgs e)
          {
               AboutWin oprograme = new();
               oprograme.Show();
          }

          private void EditujVyber(object sender, RoutedEventArgs e)
          {
               if (KnihyList.SelectedItem != null)
               {
                    var kniha = (Kniha)KnihyList.SelectedItem;
                    FormularKnihy formular = new(kniha, _zoznamAutorov);

                    var result = formular.ShowDialog() ?? false;

                    if (result)
                    {
                         int index = _aktualZoznamKnih.IndexOf(kniha);
                         ImagePreview.Source = null;
                         PomocneFunkcie.SkopirujNovyObrazok(StringKonst.ObrazkyZdrojKnihy, formular.Obrazok, kniha.Obrazok, out string novyObrazok, ref _noveKnihy);

                         int? rok;
                         if (formular.Rok != "")
                         {
                              rok = int.Parse(formular.Rok);
                         }
                         else
                         {
                              rok = null;
                         }

                         kniha = new Kniha(formular.Nazov, formular.Autor, rok,
                              formular.Vydavatelstvo, novyObrazok, formular.Popis,
                              formular.Poznamky, formular.Hodnotenie, formular.Precitana, formular.NaNeskor, formular.Kupena,
                              formular.Pozicana);

                         _aktualZoznamKnih.RemoveAt(index);
                         _aktualZoznamKnih.Insert(index, kniha);
                         _zmena = true;
                    } 
               }
               else if (AutoriList.SelectedItem != null)
               {
                    var autor = (Autor)AutoriList.SelectedItem;
                    FormularAutor formular = new(autor);

                    var result = formular.ShowDialog() ?? false;
                    if (result)
                    {
                         int index = _zoznamAutorov.IndexOf(autor);
                         ImagePreview.Source = null;
                         PomocneFunkcie.SkopirujNovyObrazok(StringKonst.ObrazkyZdrojAutori, formular.Obrazok, autor.Obrazok, out string novyObrazok, ref _noviAutori);

                         autor = new Autor(formular.Meno, formular.Narodenie, formular.Umrtie, novyObrazok, autor.ZoznamKnihAutora);
                         _zoznamAutorov.RemoveAt(index);
                         _zoznamAutorov.Insert(index, autor);
                         _kniznica.AktualizujAutora(autor);

                         KnihyList.ItemsSource = _aktualZoznamKnih;

                         _zmena = true;
                    }
               }

               PomocneFunkcie.NastavStatistiku(ref statistika, _stats);
          }

          private void KoniecProgramu(object sender, RoutedEventArgs e)
          {
               Close();
          }

          private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
          {
               if (_zmena)
               {
                    var result = MessageBox.Show("Uložiť vykonané zmeny?",
                         "Pred skončením...", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                         Dictionary<string, List<Kniha>> knihyData = [];

                         foreach (var zoznam in _kniznica.KniznicaList.Skip(1))
                         {
                              var kluc = zoznam.NazovZoznamu;
                              List<Kniha> data = [.. zoznam];
                              knihyData.Add(kluc, data);
                         }

                         string fileName = StringKonst.KnihySubor;
                         string jsonString = JsonConvert.SerializeObject(knihyData, Formatting.Indented);
                         File.WriteAllText(fileName, jsonString);

                         fileName = StringKonst.AutoriSubor;
                         jsonString = JsonConvert.SerializeObject(_zoznamAutorov, Formatting.Indented);
                         File.WriteAllText(fileName, jsonString);
                    }
                    else
                    {
                         PomocneFunkcie.OdstranNoveSuboryVratStare(StringKonst.ObrazkyZdrojKnihy, _noveKnihy);
                         PomocneFunkcie.OdstranNoveSuboryVratStare(StringKonst.ObrazkyZdrojAutori, _noviAutori);
                    }
               }
          }

          private void OtvorDetailKnihy(object sender, RoutedEventArgs? e)
          {
               if (_aktualKniha != null)
               {
                    DetailKnihy detail = new(_aktualKniha);
                    detail.ShowDialog();
               }
          }

          private void OtvorMenu(object sender, MouseButtonEventArgs e)
          {
               if (e.OriginalSource is ScrollViewer)
               {
                    KnihyList.ContextMenu = (ContextMenu)FindResource("VolneKnihyContextMenu");
                    AutoriList.ContextMenu = (ContextMenu)FindResource("VolneAutoriContextMenu");
               }
               else if (KnihyList.SelectedItem != null)
               { 
                    KnihyList.ContextMenu = (ContextMenu)FindResource("KnihaContextMenu");
               }
               else if (AutoriList.SelectedItem != null)
               {
                    AutoriList.ContextMenu = (ContextMenu)FindResource("AutorContextMenu");
               }
          }

          private void PridajAutora(object sender, RoutedEventArgs e)
          {
               FormularAutor formular = new();
               var result = formular.ShowDialog() ?? false;

               if (result)
               {
                    var autor = new Autor(formular.Meno, formular.Narodenie, formular.Umrtie, Path.GetFileName(formular.Obrazok) ?? "");

                    var filename = formular.Obrazok;
                    string destinationDirectory = StringKonst.ObrazkyZdrojAutori;

                    if (File.Exists(filename))
                    {
                         File.Copy(filename, destinationDirectory + Path.GetFileName(filename));
                         _noviAutori.Add(Path.GetFileName(filename));
                    }

                    _kniznica.PridajAutora(autor);
                    _zmena = true;
               }

               PomocneFunkcie.NastavStatistiku(ref statistika, _stats);
          }

          private void KeyOvladac(object sender, KeyEventArgs e)
          {
               if (e.Key == Key.Delete)
               {
                    OdstranObjekt(sender, e);
               } 
               else if (e.Key == Key.Enter)
               {
                    if (sender is TextBox)
                    {
                         SearchButton_OnClick(sender, e);
                    }
                    else if (AutoriList.Visibility == Visibility.Visible && AutoriList.SelectedItem != null)
                    {
                         OtvorDetailAutora(sender, null);
                    }
                    else if (KnihyList.SelectedItem != null)
                    {
                         OtvorDetailKnihy(sender, null);
                    }
               }
          }

          private void OtvorDetailAutora(object sender, RoutedEventArgs? e)
          {
               if (_aktualAutor != null)
               {
                    DetailAutora detail = new(_aktualAutor);
                    detail.ShowDialog();
               }
          }

          private void ZmenFarby(object sender, RoutedEventArgs e)
          {
               if (mainGrid.Background == null)
               {
                    mainGrid.Background = _hlavnyBrush;
                    KnihyList.Background = _vedlajsiBrush;
                    AutoriList.Background = _vedlajsiBrush;
               }
               else
               {
                    mainGrid.Background = null;
                    KnihyList.Background = null;
                    AutoriList.Background = null;
               }
               
          }

          private void SearchButton_OnClick(object sender, RoutedEventArgs e)
          {
               if (SearchBox.Text.Length != 0)
               {
                    _aktualZoznamKnih = _kniznica.ZoznamVsetkychKnih.NajdiPodlaPodmienky(kniha =>
                         kniha.Nazov.ToLower().Contains(SearchBox.Text.ToLower()) || kniha.AutorKnihy.ToLower().Contains(SearchBox.Text.ToLower()));

                    AutoriList.Visibility = Visibility.Hidden;
                    KnihyList.Visibility = Visibility.Visible;
                    KnihyList.SelectedItem = null;
                    ImagePreview.Source = null;
                    KnihyList.ItemsSource = _aktualZoznamKnih;
                    NazovZoznamu.Content = StringKonst.Vyhladavanie;
               }
          }
     }
}