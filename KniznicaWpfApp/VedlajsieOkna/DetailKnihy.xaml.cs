using System.Windows.Media;
using System.Windows.Media.Imaging;
using KniznicaLibrary;

namespace KniznicaWpfApp
{
    /// <summary>
    /// Interaction logic for DetailKnihy.xaml
    /// </summary>
    public partial class DetailKnihy
    {
        public DetailKnihy(Kniha kniha)
        {
            InitializeComponent();
            DataContext = kniha;

            BitmapImage imagebit = new();
            imagebit.BeginInit();
            imagebit.CacheOption = BitmapCacheOption.OnLoad;
            if (kniha.Obrazok != "")
            {
                 imagebit.UriSource = new Uri($"Resources/KnihyObrazky/{kniha.Obrazok}", UriKind.Relative);
            }
            else
            {
                 imagebit.UriSource = new Uri("Resources/KnihyObrazky/defaultB.png", UriKind.Relative);
            }

            imagebit.EndInit();
            image.Source = imagebit;

            if (kniha.Hodnotenie > 7)
            {
                 hodnotenie.Foreground = Brushes.ForestGreen;
            } else if (kniha.Hodnotenie < 4)
            {
                 hodnotenie.Foreground = Brushes.Red;
            }
            else
            {
                 hodnotenie.Foreground = Brushes.Orange;
            }

            grid.Background = Brushes.AntiqueWhite;
        }
    }
}
