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
        public class TaskItem
        {
            public string teamName { get; set; }
            public string status { get; set; }
            public string text { get; set; }
            public int teamID { get; set; }
        }
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
        /*private void ShowTasks()
        {
            TaskListResponse listTodo = new TaskListResponse();
            listTodo = GlobalUsage.Client().GetTeamSpecificTasks(new TeamDescription { Index = GlobalUsage.CurrentUser.workerID, Name = "todo" });
            
            foreach(var i in listTodo.Tasks)
            {
                todo.Items.Add(i.Text);

            }

            TaskListResponse listInprogress = new TaskListResponse();
            listInprogress = GlobalUsage.Client().GetTeamSpecificTasks(new TeamDescription { Index = GlobalUsage.CurrentUser.workerID, Name = "inprogress" });

            foreach (var i in listInprogress.Tasks)
            {
                inprogress.Items.Add(i.Text);

            }

            TaskListResponse listTesting = new TaskListResponse();
            listTesting = GlobalUsage.Client().GetTeamSpecificTasks(new TeamDescription { Index = GlobalUsage.CurrentUser.workerID, Name = "testing" });

            foreach (var i in listTesting.Tasks)
            {
                testing.Items.Add(i.Text);

            }

            TaskListResponse listDone = new TaskListResponse();
            listDone = GlobalUsage.Client().GetTeamSpecificTasks(new TeamDescription { Index = GlobalUsage.CurrentUser.workerID, Name = "done" });

            foreach (var i in listDone.Tasks)
            {
                done.Items.Add(i.Text);

            }
        }*/
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            int _level = GlobalUsage.CurrentUser.level;
            if (_level == 2)
            {
                DeptTasks.Visibility = Visibility.Visible;
            }
            else
            {
                DeptTasks.Visibility = Visibility.Hidden;
            }
            team.Content = GlobalUsage.CurrentUser.teamName;
            //ShowTasks();
            Update();
         
        }

        private void item_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!Pops.IsOpen)
            {
                Pops.DataContext = (sender as FrameworkElement).DataContext;
                Pops.PlacementTarget = (sender as UIElement);
                Pops.IsOpen = true;
            }
        }

        private void item_MouseLeave(object sender, MouseEventArgs e)
        {
            var textB = sender as FrameworkElement;
            if (textB.IsMouseOver || Pops.IsMouseOver) return;
            Pops.IsOpen = false;
        }

        private void Update()
        {
            var tasks = GlobalUsage.GetInfClient().GetTaskList(new MessagesPack.TaskListRequest { TeamName = GlobalUsage.CurrentUser.teamName });
            var lsTodo = new List<TaskItem>();
            var lsInprogress = new List<TaskItem>();
            var lsTesting = new List<TaskItem>();
            var lsDone = new List<TaskItem>();
            foreach ( var i in tasks.Tasks)
            {
                if(i.Status == "todo")
                {
                    lsTodo.Add(new TaskItem { teamName = i.Team, status = i.Status, teamID = i.TeamID, text = i.Text });
                }
                else if (i.Status == "inprogress")
                {
                    lsInprogress.Add(new TaskItem { teamName = i.Team, status = i.Status, teamID = i.TeamID, text = i.Text });
                }
                else if (i.Status == "testing")
                {
                    lsTesting.Add(new TaskItem { teamName = i.Team, status = i.Status, teamID = i.TeamID, text = i.Text });
                }
                else if (i.Status == "done")
                {
                    lsDone.Add(new TaskItem { teamName = i.Team, status = i.Status, teamID = i.TeamID, text = i.Text });
                }
                ListT.ItemsSource = lsTesting;
                ListTD.ItemsSource = lsTodo;
                ListIP.ItemsSource = lsInprogress;
                ListD.ItemsSource = lsDone;
            }
        }
    }
}
