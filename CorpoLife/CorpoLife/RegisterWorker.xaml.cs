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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegisterWorker : Window
    {
        public RegisterWorker()
        {
            InitializeComponent();
        }

        public string DepName, TeamName, Name, Password;
        public int Level;
        
        private void DepartmentSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //remember chosen department
            DepName = ((ComboBoxItem)TeamSelection.SelectedItem).Content.ToString();
            //get teams for this department and show them in ComboBox
            var teamList = GlobalUsage.GetInfClient().GetDepartmetTeams(new NameRequest { TeamName = DepName });
            foreach (var t in teamList.TeamDesc)
            {
                TeamSelection.Items.Add(t.Name);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            GetName();
            GetPassword();
            var resp = GlobalUsage.GetIntClient().Register(new RegisterRequest { Level = Level, Name = Name, Password = Password, TeamName = TeamName, DepName = DepName });
            MessageBox.Show(resp.Msg);
        }

        private void GetName()
        {
            Name = NameSelection.Text;

        }
        private void GetPassword()
        {
            Password = PasswordSelection.Password;
        }
        private void TeamSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //remember chosen team
            TeamName = ((ComboBoxItem)TeamSelection.SelectedItem).Content.ToString();
            
        }

        private void LevelSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //remember selected Level
            Level = ((ComboBoxItem)LevelSelection.SelectedItem).TabIndex + 1;

        }


        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            //get department list and show them in ComboBox
            var depList = GlobalUsage.GetInfClient().GetDepartments(new MessagesPack.BlankMsg());
            foreach (var dep in depList.DepsDesc)
            {
                DepartmentSelection.Items.Add(dep.Name);
            }
        }
    }
}
