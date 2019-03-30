﻿using System;
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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class WorkerView : Window
    {
        public WorkerView()
        {
            //InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TeamTaskWindow tasks = new TeamTaskWindow();
            tasks.Show();
        }
    }
}
