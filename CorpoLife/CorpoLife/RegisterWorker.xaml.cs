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

        public string depName, teamName, name, password, level;

        
        private void DepartmentSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get department list and show them in ComboBox
            var client = CorpoLife.GlobalUsage.Client();
            var depList = client.GetDepartments(new MessagesPack.BlankMsg());
            foreach (var dep in depList.DepsDesc)
            {
                DepartmentSelection.Items.Add(dep.Name);
            }
            //remember chosen department
            depName = ((ComboBoxItem)TeamSelection.SelectedItem).Content.ToString();
            
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            GetName();
            GetPassword();
            var client = CorpoLife.GlobalUsage.Client();//TODO change to level
            var resp = client.Register(new RegisterRequest {Level = 1, Name = name, Password = password, Team = "bc" });
            MessageBox.Show(resp.Msg);
        }

        private void GetName()
        {
            name = NameSelection.Text;

        }
        private void GetPassword()
        {
            password = PasswordSelection.Password;
        }
        private void TeamSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get teams for this department and show them in ComboBox
            var client = CorpoLife.GlobalUsage.Client();
            var teamList = client.GetDepartmetTeams(new NameRequest {TeamName = depName });
            foreach (var t in teamList.TeamDesc)
            {
                TeamSelection.Items.Add(t.Name);
            }
            //remember chosen team
            teamName = ((ComboBoxItem)TeamSelection.SelectedItem).Content.ToString();
            
        }

        private void LevelSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //remember selected level
            level = ((ComboBoxItem)LevelSelection.SelectedItem).Content.ToString();

        }

        
    }
}
