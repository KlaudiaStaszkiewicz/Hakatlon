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
using MessagesPack;

namespace CorpoLife
{
    /// <summary>
    /// Interaction logic for HeadsOverview.xaml
    /// </summary>
    public partial class HeadsOverview : Window
    {
        DepartmentsListResp depList;
        ServerEvents.ServerEventsClient client;
        public HeadsOverview()
        {
            InitializeComponent();
        }

        void UpdateView()
        {
            Button tmpButton = new Button();
            tmpButton.Height = 60;
            tmpButton.Width = 100;
            int LastLeftCornerX = 10;
            int LastLeftCornerY = 10;
            foreach(var item in depList.DepsDesc)
            {
                tmpButton = new Button();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                tmpButton.Margin = new Thickness(LastLeftCornerX, LastLeftCornerY, 0, 0);
                tmpButton.Background = new SolidColorBrush(Color.FromArgb(50, (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160))));
                tmpButton.Name = "button_" + item.Name;
                tmpButton.Content = item.Name + "\n" + client.GetDepHead(new DepartmentDescription { Index = item.Index, Name = item.Name }).Name;
                MainGrid.Children.Add(tmpButton);
            }            
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            client = GlobalUsage.Client();
            depList = client.GetDepartments(new BlankMsg { });
            UpdateView();
        }
        private void NewDep_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NamePrompt("Add new department", "hello");

            dialog.Closing += (_sender, _e) =>

            {
                var d = _sender as NamePrompt;
                if (!d.Canceled)
                {
                    client.AddDepartment(new AddDepRequest { DepName = d.InputText });
                }
            };
            dialog.Show();
        }
    }
}
