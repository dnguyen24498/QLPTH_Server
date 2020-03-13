using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TabMenu
    {
        public TabMenu()
        {
            this.Items = new ObservableCollection<TabMenuItem>();
        }
        public string Title { get; set; }
        public string Icon { get; set; }
        public ObservableCollection<TabMenuItem> Items { get; set; }
    }
}
