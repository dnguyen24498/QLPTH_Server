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

namespace View
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }


        private void btnKetNoi_Click(object sender, RoutedEventArgs e)
        {
            lbSettings.ScrollIntoView(lbSettings.Items[0]);
        }

        private void btnThongBao_Click(object sender, RoutedEventArgs e)
        {
            lbSettings.ScrollIntoView(lbSettings.Items[1]);
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            lbSettings.ScrollIntoView(lbSettings.Items[2]);
        }
    }
}
