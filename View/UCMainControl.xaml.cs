using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
    /// <summary>
    /// Interaction logic for UCDangNhap.xaml
    /// </summary>
    public partial class UCMainControl : UserControl
    {
        public UCMainControl()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)lbNotification.Items).CollectionChanged += UCMainControl_CollectionChanged;
        }

        private void UCMainControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                lbNotification.ScrollIntoView(e.NewItems[0]);
            }
        }
    }
}
