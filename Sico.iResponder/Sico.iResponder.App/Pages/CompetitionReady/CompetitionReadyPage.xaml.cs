using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sico.iResponder.App.Pages.CompetitionReady
{
    /// <summary>
    /// Interaction logic for CompetitionReadyPage.xaml
    /// </summary>
    public partial class CompetitionReadyPage : Page
    {
        public event Action TimeUp;
        public CompetitionReadyPage()
        {
            InitializeComponent();
        }

        public void StartCountDown()
        {
            CountDownImage.Source = new BitmapImage(new Uri("3.png", UriKind.Relative));
            Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                this.Dispatcher.Invoke(() =>
                {
                    CountDownImage.Source = new BitmapImage(new Uri("2.png", UriKind.Relative));
                });
                Task.Delay(1000).Wait();
                this.Dispatcher.Invoke(() =>
                {
                    CountDownImage.Source = new BitmapImage(new Uri("1.png", UriKind.Relative));
                });
                Task.Delay(1000).Wait();
                this.Dispatcher.Invoke(() =>
                {
                    TimeUp?.Invoke();
                });
            });
        }
    }
}
