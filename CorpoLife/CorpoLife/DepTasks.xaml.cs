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
    /// Interaction logic for DepTasks.xaml
    /// </summary>
    public partial class DepTasks : Window
    {
        ServerEvents.ServerEventsClient client = GlobalUsage.Client();
        TaskListResponse taskList = new TaskListResponse();
        public DepTasks()
        {
            InitializeComponent();
        }
        void UpdateVisuals()
        {
            var tmpButton = new Label();
            tmpButton.Height = 60;
            tmpButton.Width = 100;
            int LastLeftCornerX = 10;
            int LastLeftCornerY = 10;
            foreach (var item in taskList.Tasks)
            {
                tmpButton = new Label();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                tmpButton.Margin = new Thickness(LastLeftCornerX, LastLeftCornerY, 0, 0);
                tmpButton.Background = new SolidColorBrush(Color.FromArgb(50, (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160))));
                tmpButton.HorizontalAlignment = HorizontalAlignment.Left;
                tmpButton.VerticalAlignment = VerticalAlignment.Top;
                tmpButton.Content = item.Text + "\n" + item.Status;
                MainGrid.Children.Add(tmpButton);
                LastLeftCornerX += new Random().Next(-80, 150);
                LastLeftCornerY += new Random().Next(40, 80);
            }
        }
        void Update()
        {
            taskList = client.GetAllDepTasks(new MessagesPack.NameRequest { TeamName = (client.GetDepFromUser(new MessagesPack.IntegerRequest { Number = GlobalUsage.currentUser.workerID }).Msg) });
            UpdateVisuals();
        }
        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
