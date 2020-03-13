using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public static class ManageSettings
    {
        public static bool Notification
        {
            get => Properties.Settings.Default.Notification;
            set
            {
                Properties.Settings.Default.Notification = value;
                Properties.Settings.Default.Save();
            }
        }
        public static bool NotificationSound
        {
            get => Properties.Settings.Default.NotificationSound;
            set
            {
                Properties.Settings.Default.NotificationSound = value;
                Properties.Settings.Default.Save();
            }
        }
        public static string RoomID
        {
            get => Properties.Settings.Default.ClassroomID;
            set
            {
                Properties.Settings.Default.ClassroomID = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
