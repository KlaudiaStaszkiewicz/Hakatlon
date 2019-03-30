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
using System.Windows.Shapes;

namespace CorpoLife
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
        }
        

        private void Coffee_Click(object sender, RoutedEventArgs e)
        {
        }


        private void Workers_Click(object sender, RoutedEventArgs e)
        {
            WorkerOverview workerOver = new WorkerOverview();
            workerOver.Show();
        }

        private void Tasks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Deps_Click(object sender, RoutedEventArgs e)
        {
            HeadsOverview headsOver = new HeadsOverview();
            headsOver.Show();
        }

        private void Ems_Click(object sender, RoutedEventArgs e)
        {

        }

        private void More_Click_1(object sender, RoutedEventArgs e)
        {
            if (Coffee.Visibility == Visibility.Hidden)
            {
                Coffee.Visibility = Visibility.Visible;
                Callendar.Visibility = Visibility.Visible;
                Workers.Visibility = Visibility.Visible;
                Map.Visibility = Visibility.Visible;
            }
            else
            {
                Coffee.Visibility = Visibility.Hidden;
                Callendar.Visibility = Visibility.Hidden;
                Workers.Visibility = Visibility.Hidden;
                Map.Visibility = Visibility.Hidden;
            }
        }
    }
}
