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
    /// Interaction logic for HeadTaskWindow.xaml
    /// </summary>
    public partial class HeadTaskWindow : Window
    {
        DepartmentsListResp depList;
        ServerEvents.ServerEventsClient client;
        public HeadTaskWindow()
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
            foreach (var item in depList.DepsDesc)
            {
                tmpButton = new Button();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                tmpButton.Margin = new Thickness(LastLeftCornerX, LastLeftCornerY, 0, 0);
                tmpButton.Background = new SolidColorBrush(Color.FromArgb(50, (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160))));
                tmpButton.Name = "button_" + item.Name;
                tmpButton.HorizontalAlignment = HorizontalAlignment.Left;
                tmpButton.VerticalAlignment = VerticalAlignment.Top;
                tmpButton.Content = item.Name + "\n" + client.GetDepHead(new DepartmentDescription { Index = item.Index, Name = item.Name }).Name;
                MainGrid.Children.Add(tmpButton);
                LastLeftCornerX += new Random().Next(-40, 150);
                LastLeftCornerY += new Random().Next(65, 80);
            }
        }

        /*private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            client = GlobalUsage.Client();
            depList = client.GetDepartments(new BlankMsg { });
            UpdateView();
        }

        private void DepTasks_Click(object sender, RoutedEventArgs e)
        {
            DepTasks dep = new DepTasks();
            dep.Show();
        }

        private void TeamOverview_Click(object sender, RoutedEventArgs e)
        {
            var tmpButton = new Button();
            tmpButton.Height = 60;
            tmpButton.Width = 100;
            int LastLeftCornerX = 10;
            int LastLeftCornerY = 10;
            var teamslist = GlobalUsage.Client().GetDepartmetTeams(new NameRequest { TeamName = GlobalUsage.currentUser.teamName });
            foreach(var item in teamslist.TeamDesc)
            {
                tmpButton = new Button();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                tmpButton.Margin = new Thickness(LastLeftCornerX, LastLeftCornerY, 0, 0);
                tmpButton.Background = new SolidColorBrush(Color.FromArgb(50, (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160))));
                tmpButton.HorizontalAlignment = HorizontalAlignment.Left;
                tmpButton.VerticalAlignment = VerticalAlignment.Top;
                tmpButton.Content = item.Name;
                MainGrid.Children.Add(tmpButton);
                LastLeftCornerX += new Random().Next(-80, 150);
                LastLeftCornerY += new Random().Next(40, 80);
            }
        }*/
        private void DepTasks_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
