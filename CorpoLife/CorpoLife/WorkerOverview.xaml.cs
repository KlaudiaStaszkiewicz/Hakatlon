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

namespace CorpoLife
{  

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WorkerOverview : Window
    {
        public WorkerOverview()
        {
            /*InitializeComponent();
            var listOfWorkers = GlobalUsage.Client();

            foreach(var w in listOfWorkers.items)
            {
                listW.Items.Add(w.Name);
            }*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegisterWorker register = new RegisterWorker();
            register.Show();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageBox.Show("I will find the worker you are looking for!");
        }
        
    }
}
