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
        public class DepsListItem
        {
            public int Id;
            public string Name;
            public string Head;
            public int NumOfMembers;
            public int NumOfTeams;

            public DepsListItem(int id, string name, int numOfTeams, int numOfMembers, string head = "")
            {
                Id = id;
                Name = name;
                Head = head;
                NumOfMembers = numOfMembers;
                NumOfTeams = numOfTeams;
            }
        }
        private DepartmentsListResp _depList;
        public HeadsOverview()
        {
            InitializeComponent();
        }

        void UpdateView()
        {
            var itemList = new List<DepsListItem>();

            foreach (var w in _depList.DepsDesc)
            {
                itemList.Add(new DepsListItem(w.Index, w.Name,
                    GlobalUsage.GetInfClient().GetDepartmetTeams(new NameRequest() { TeamName = w.Name}).TeamDesc.Count,
                    GlobalUsage.GetInfClient().GetDepartmentWorkers(new NameRequest() { TeamName = w.Name }).TeamDesc.Count,
                    GlobalUsage.GetInfClient().GetDepHead(new DepartmentDescription(){Index = w.Index, Name = w.Name}).Name));
            }
            DepsList.ItemsSource = itemList;
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            _depList = GlobalUsage.GetInfClient().GetDepartments(new BlankMsg() {});
            UpdateView();
        }
        private void NewDep_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new NamePrompt("Add new department", "");

            dialog.Closing += (_sender, _e) =>

            {
                var d = _sender as NamePrompt;
                if (!d.Canceled)
                {
                    GlobalUsage.GetIntClient().AddDepartment(new AddDepRequest { DepName = d.InputText });
                }
            };
            dialog.Show();
        }
    }
}
