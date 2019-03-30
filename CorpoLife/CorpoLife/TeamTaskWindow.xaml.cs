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
    /// Interaction logic for TeamTaskWindow.xaml
    /// </summary>
    public partial class TeamTaskWindow : Window
    {
        public TeamTaskWindow()
        {
            InitializeComponent();
            Button tmpButton = new Button();
            tmpButton.Height = 60;
            tmpButton.Width = 100;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DeptTasks_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ShowTasks()
        {
            TaskListResponse listOfTasks = new TaskListResponse();
            
            foreach (var item in listOfTasks.Tasks)
            {
                Button tmpButton = new Button();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                int LastLeftCornerX = 10;
                int LastLeftCornerY = 10;
                tmpButton = new Button();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                tmpButton.Margin = new Thickness(LastLeftCornerX, LastLeftCornerY, 0, 0);
                tmpButton.Background = new SolidColorBrush(Color.FromArgb(50, (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160))));
                tmpButton.Name = "button_" + item.Name;
                tmpButton.Content = item.Name + "\n" + client.GetDepHead(new DepartmentDescription { Index = item.Index, Name = item.Name }).Name;
                Grid.Children.Add(tmpButton);
            }
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            int _level = GlobalUsage.currentUser.level;
            if (_level == 2)
            {
                DeptTasks.Visibility = Visibility.Visible;
            }
            else
            {
                DeptTasks.Visibility = Visibility.Hidden;
            }
            ShowTasks();
         
        }
    }
}
