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
            HeadTaskWindow head = new HeadTaskWindow();
            head.Show();
        }
        private void ShowTasks()
        {
            TaskListResponse listTodo = new TaskListResponse();
            listTodo = GlobalUsage.Client().GetTeamSPecificTasks(new TeamDescription { Index = GlobalUsage.currentUser.workerID, Name = "todo" });
            
            foreach(var i in listTodo.Tasks)
            {
                todo.Items.Add(i.Text);

            }

            TaskListResponse listInprogress = new TaskListResponse();
            listInprogress = GlobalUsage.Client().GetTeamSPecificTasks(new TeamDescription { Index = GlobalUsage.currentUser.workerID, Name = "inprogress" });

            foreach (var i in listInprogress.Tasks)
            {
                inprogress.Items.Add(i.Text);

            }

            TaskListResponse listTesting = new TaskListResponse();
            listTesting = GlobalUsage.Client().GetTeamSPecificTasks(new TeamDescription { Index = GlobalUsage.currentUser.workerID, Name = "testing" });

            foreach (var i in listTesting.Tasks)
            {
                testing.Items.Add(i.Text);

            }

            TaskListResponse listDone = new TaskListResponse();
            listDone = GlobalUsage.Client().GetTeamSPecificTasks(new TeamDescription { Index = GlobalUsage.currentUser.workerID, Name = "done" });

            foreach (var i in listDone.Tasks)
            {
                done.Items.Add(i.Text);

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
