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
using MessagesPack;

namespace CorpoLife
{  

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WorkerOverview : Window
    {
        public class WorkerItem
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
        public WorkerOverview()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWorker().Show();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO make search working
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            var listOfWorkers = GlobalUsage.GetInfClient().GetAllWorkers(new BlankMsg());
            var itemList = new List<WorkerItem>();
            foreach (var w in listOfWorkers.TeamDesc)
            {
                itemList.Add(new WorkerItem() { Name = w.Name, Id = w.Index });
            }

            listW.ItemsSource = itemList;
        }
    }
}
