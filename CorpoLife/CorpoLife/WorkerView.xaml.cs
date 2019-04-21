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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class WorkerView : Window
    {
        public class LeaderListItem
        {
            public int LID { get; set;}
            public int LteamID { get; set; }
            public string LName { get; set; }
            public string LteamName { get; set; }
        }
        public WorkerView()
        {
            InitializeComponent();
        }

        private void Tasks_Click(object sender, RoutedEventArgs e)
        {
            TeamTaskWindow tasks = new TeamTaskWindow();
            tasks.Show();
        }
        
        private void Ems_Click(object sender, RoutedEventArgs e)
        {
            Emergency em = new Emergency();
            em.Show();
            //GlobalUsage.Client().CallEmergency(new MessagesPack.IntIntRequest { TeamID = GlobalUsage.CurrentUser.teamID, WorkerID = GlobalUsage.CurrentUser.workerID });
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LeadersList.Visibility = Visibility.Hidden;
            Close.Visibility = Visibility.Hidden;
            int _level = GlobalUsage.CurrentUser.level;
            if(_level == 2)
            {
                Ems.Visibility = Visibility.Visible;
                Leads.Visibility = Visibility.Visible;
            }
            else
            {
                Ems.Visibility = Visibility.Hidden;
                Leads.Visibility = Visibility.Hidden;
            }

        }

        private void Coffee_Click(object sender, RoutedEventArgs e)
        {
            GlobalUsage.GetRtClient().PullCoffeeBrake(new MessagesPack.CoffeBreakRequest() { Name = GlobalUsage.CurrentUser.name});
            
        }
        private void Leaders_Click(object sender, RoutedEventArgs e)
        {
            LeadersList.Visibility = Visibility.Visible;
            Close.Visibility = Visibility.Visible;
            var ls = GlobalUsage.GetInfClient().GetLeaders(new MessagesPack.BlankMsg());
            var lsItems = new List<LeaderListItem>();
            foreach(var l in ls.Leaders)
            {
                lsItems.Add(new LeaderListItem { LID = l.LeaderId, LName = l.LeaderName, LteamID = l.TeamId, LteamName = l.TeamName});
            }
            LeadersList.ItemsSource = lsItems;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            LeadersList.Visibility = Visibility.Hidden;
            Close.Visibility = Visibility.Hidden;
        }
    }
}
