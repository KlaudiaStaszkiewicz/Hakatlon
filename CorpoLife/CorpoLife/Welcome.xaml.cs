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
        }
    }
}
