using System;
using System.Windows;
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
            //TODO response.State instead of true
            //var response = GlobalUsage.GetRtClient().LogIn(new MessagesPack.LoginRequest { Id = Convert.ToInt32(IdText), Password = PasswordText });
            //response.State
            if (true)
            {
                Canceled = false;
                Close();
            }
        }

        // private void LogIn_Click(object sender, RoutedEventArgs e)
        // {
        //     int level = GlobalUsage.CurrentUser.level;
        //
        //     switch (level)
        //     {
        //         case 1:
        //             WorkerView workerwelcome = new WorkerView();
        //             workerwelcome.Show();
        //             break;
        //         case 2:
        //             WorkerView leaderwelcome = new WorkerView();
        //             leaderwelcome.Show();
        //             break;
        //         case 3:
        //             HeadView headwelcome = new HeadView();
        //             headwelcome.Show();
        //             break;
        //         case 4:
        //             HeadView adminwelcome = new HeadView();
        //             adminwelcome.Show();
        //             break;
        //
        //     }
        //     this.Close();
        //
        // }
        //
        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dialog = new LoginPrompt();
            var client = GlobalUsage.GetRtClient();
            dialog.Closing += (_sender, _e) =>
            {

                var d = _sender as LoginPrompt;
                if (!(d.Canceled))
                {

                    var id = Convert.ToInt32(d.InputTextBox.Text);
                    var password = d.PasswrdInput.Password;

                    /*var response = client.LogIn(new LoginRequest {Id = id, Password = password});
                    if (response.State)
                    {
                        GlobalUsage.CurrentUser.level = response.Level;
                        GlobalUsage.CurrentUser.workerID = id;
                        GlobalUsage.CurrentUser.coffee = false;
                        GlobalUsage.CurrentUser.emergency = false;
                        GlobalUsage.CurrentUser.teamName = response.Team;
                        GlobalUsage.CurrentUser.name = response.Name;
                        GlobalUsage.CurrentUser.teamID = response.TeamId;
                        */
                    GlobalUsage.CurrentUser.level = 2;
                    switch (GlobalUsage.CurrentUser.level)
                        {
                            case 1:
                                var workerWelcome = new WorkerView();
                                workerWelcome.Show();
                                break;
                            case 2:
                                var leaderWelcome = new WorkerView();
                                leaderWelcome.Show();
                                break;
                            case 3:
                                var headWelcome = new HeadView();
                                headWelcome.Show();
                                break;
                            case 4:
                                var adminWelcome = new HeadView();
                                adminWelcome.Show();
                                break;
                        }

                        Close();
                    //}
                }
            };
            dialog.Show();
        }
    }
}
