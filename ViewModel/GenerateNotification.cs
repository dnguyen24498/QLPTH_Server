using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ViewModel
{
    public class GenerateNotification
    {
        public static void ShowNoti(string title,string content,NotificationType type)
        {
            if (ManageSettings.Notification)
            {
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = title,
                    Message = content,
                    Type = type
                });
            }
            if (ManageSettings.NotificationSound)
            {
                PlaySound.Play();
            }
        }
    }
}
