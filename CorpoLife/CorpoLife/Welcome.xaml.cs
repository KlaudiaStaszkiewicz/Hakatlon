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
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
        }

        
        
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            int level = GlobalUsage.currentUser.level;

            switch (level)
            {
                case 1:
                    WorkerView workerwelcome = new WorkerView();
                    workerwelcome.Show();
                    break;
                case 2:
                    WorkerView leaderwelcome = new WorkerView();
                    leaderwelcome.Show();
                    break;
                case 3:
                    HeadView headwelcome = new HeadView();
                    headwelcome.Show();
                    break;
                case 4:
                    HeadView adminwelcome = new HeadView();
                    adminwelcome.Show();
                    break;

            }
            this.Close();

        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dialog = new LoginPrompt();
            var client = GlobalUsage.Client();
            dialog.Closing += (_sender, _e) =>
            {
                var d = _sender as LoginPrompt;
                if (!(d.Canceled))
                {
                    int level = GlobalUsage.currentUser.level;

                    switch (level)
                    {
                        case 1:
                            WorkerView workerwelcome = new WorkerView();
                            workerwelcome.Show();
                            break;
                        case 2:
                            WorkerView leaderwelcome = new WorkerView();
                            leaderwelcome.Show();
                            break;
                        case 3:
                            HeadView headwelcome = new HeadView();
                            headwelcome.Show();
                            break;
                        case 4:
                            HeadView adminwelcome = new HeadView();
                            adminwelcome.Show();
                            break;
                    }
                    Close();
                }
            };
            dialog.Show();
        }
    }
}
