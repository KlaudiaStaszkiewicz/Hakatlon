using System;
using System.Windows;


namespace CorpoLife
{
    /// <summary>
    /// Interaction logic for LoginPrompt.xaml
    /// </summary>
    public partial class LoginPrompt : Window
    {
        public LoginPrompt()
        {
            InitializeComponent();
        }

        public string PasswordText
        {
            get => PasswrdInput.Text;
            set => PasswrdInput.Text = value;
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
            var response = GlobalUsage.GetRtClient().LogIn(new MessagesPack.LoginRequest { Id = Convert.ToInt32(IdText), Password = PasswordText });
            if (response.State)
            {
                Canceled = false;
                Close();
            }
        }
    }
}
