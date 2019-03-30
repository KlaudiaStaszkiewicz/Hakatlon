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
    /// Interaction logic for GlobalTaskWin.xaml
    /// </summary>
    public partial class GlobalTaskWin : Window
    {
        public GlobalTaskWin()
        {
            InitializeComponent();
            var client = CorpoLife.GlobalUsage.Client();
            var depList = client.GetDepartments(new MessagesPack.BlankMsg());
            foreach (var dep in depList.DepsDesc)
            {
                Button tmpButton = new Button();
                tmpButton.Height = 60;
                tmpButton.Width = 100;
                tmpButton.Margin = new Thickness(200, 150, 0, 0);
                tmpButton.Background = new SolidColorBrush(Color.FromArgb(50, (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160)), (byte)(new Random().Next(55, 160))));
                tmpButton.Name = "button_" + dep.Name;
                tmpButton.Content = dep.Name;
                Place1.Children.Add(tmpButton);

            }
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
