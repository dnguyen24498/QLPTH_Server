using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UCChatBox.xaml
    /// </summary>
    public partial class UCChatBox : UserControl
    {
        public UCChatBox()
        {
                InitializeComponent();
            ((INotifyCollectionChanged)lbMessenger.Items).CollectionChanged += UCChatBox_CollectionChanged;
        }

        private void UCChatBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (VisualTreeHelper.GetChildrenCount(lbMessenger) > 0)
                {
                    Border border = (Border)VisualTreeHelper.GetChild(lbMessenger, 0);
                    ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                    scrollViewer.ScrollToBottom();
                }
            }
        }

    }
   
}
