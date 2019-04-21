using System;
using System.Windows;
using System.Windows.Input;
using MessagesPack;


namespace CorpoLife
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
        }

        public string PasswordText
        {
            get => PasswrdInput.Password;
            set => PasswrdInput.Password = value;
        }

        public string IdText
        {
            get => InputTextBox.Text;
            set => InputTextBox.Text = value;
        }

        public bool Canceled { get; set; }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Canceled = true;
            Close();
        }
        private void BtnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
                CheckLogIn();
            
        }
        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CheckLogIn();
            }
        }

        private void CheckLogIn()
        {
            var response = GlobalUsage.GetRtClient().LogIn(new LoginRequest { Id = Convert.ToInt32(IdText), Password = PasswordText });
            if (response.State)
            {
                GlobalUsage.CurrentUser.name = response.Name;
                GlobalUsage.CurrentUser.level = response.Level;
                GlobalUsage.CurrentUser.teamID = response.TeamId;
                GlobalUsage.CurrentUser.teamName = response.Team;
                GlobalUsage.CurrentUser.workerID = Convert.ToInt32(IdText);
                GlobalUsage.CurrentUser.coffee = false;
                GlobalUsage.CurrentUser.emergency = false;
                GlobalUsage.CurrentUser.status = 1;
                Canceled = false;
                switch (GlobalUsage.CurrentUser.level)
                {
                    case 0:
                    case 1:
                    case 2:
                        new WorkerView().Show();
                        break;
                    case 3:
                        new AdminView().Show();
                        break;
                }
                Close();
            }
        }
    }
}
