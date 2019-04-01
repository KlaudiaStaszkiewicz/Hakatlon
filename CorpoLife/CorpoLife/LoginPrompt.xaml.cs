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
            get { return PasswrdInput.Text; }
            set { PasswrdInput.Text = value; }
        }

        public string IdText
        {
            get { return InputTextBox.Text; }
            set { InputTextBox.Text = value; }
        }

        public bool Canceled { get; set; }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Canceled = true;
            Close();
        }

        private void BtnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var client = GlobalUsage.Client();
            //var response = client.LogIn(new MessagesPack.LoginRequest { Id = Convert.ToInt32(IdText), Password = PasswordText });
            //if (response.State)
            if(true)
            {
                Canceled = false;
                Close();
            }
        }
    }
}
