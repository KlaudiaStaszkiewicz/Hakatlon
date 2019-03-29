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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegisterWorker : Window
    {
        public RegisterWorker()
        {
            InitializeComponent();
        }
        class tempDepartment
        {
            public int departmentID;
            public string departmentName;
        }
        class tempTeam
        {
            public int teamID;
            public string teamName;
        }

        private void DepartmentSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            //TODO get department list
            List<tempDepartment> Departments = new List<tempDepartment>();
            foreach(tempDepartment i in Departments)
            {
                DepartmentSelection.Items.Add(i.departmentName);
            }
            string department = ((ComboBoxItem)TeamSelection.SelectedItem).Content.ToString();
            //TODO send info about department for this worker
        }

        private void TeamSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO get teams for this department
            List<tempTeam> Teams = new List<tempTeam>();
            foreach (tempTeam i in Teams)
            {
                TeamSelection.Items.Add(i.teamName);
            }
            string team = ((ComboBoxItem)TeamSelection.SelectedItem).Content.ToString();
            //TODO send info about team for this worker
        }

        private void LevelSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string level = ((ComboBoxItem)LevelSelection.SelectedItem).Content.ToString();

        }
    }
}
