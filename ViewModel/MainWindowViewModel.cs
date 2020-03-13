using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VMHelper;
using Server;
using GeneralClass;
using Model;
namespace ViewModel
{
    public class MainWindowViewModel:BaseViewModel
    {
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ClosedWindowCommand { get; set; }

        private string _areaName;
        public string AreaName
        {
            get => _areaName;
            set
            {
                _areaName = value;
                OnPropertyChanged();
            }
        }

        private string _floorName;
        public string FloorName
        {
            get => _floorName;
            set
            {
                _floorName = value;
                OnPropertyChanged();
            }
        }

        private string _classroomName;
        public string ClassroomName
        {
            get => _classroomName;
            set
            {
                _classroomName = value;
                OnPropertyChanged();
            }
        }

        private string _classroomStatus;
        public string ClassroomStatus
        {
            get => _classroomStatus;
            set
            {
                _classroomStatus = value;
                OnPropertyChanged();
            }
        }

        private void Instance_BroadcastNotification(object sender, GeneralClass.MessageArgs e)
        {
            GenerateNotification.ShowNoti("Thông báo mới", e.Content, Notifications.Wpf.NotificationType.Information);
        }

        public MainWindowViewModel()
        {
            AreaName = "--";
            FloorName = "--";
            ClassroomName = "--";
            ClassroomStatus = "--";
            LoadedWindowCommand = new RelayCommand<object>((p) => true, (p) => {
                GenerateServer();
                updateRoomInformationAsync();
            });
            ClosedWindowCommand = new RelayCommand<object>((p) => true, (p) => {
                Server.Server.Instance.Close();
            });

        }

        private void GenerateServer()
        {
            Server.Server.Instance.Start();
            Server.Server.Instance.BroadcastNotification += Instance_BroadcastNotification;
        }

        private async void updateRoomInformationAsync()
        {
            GenerateNotification.ShowNoti("Thông báo mới", "Đang kết nối tới máy chủ dữ liệu...", Notifications.Wpf.NotificationType.Information);
            await Task.Run(() => getAndUpdateRoomInformationFromDB());
        }

        private void getAndUpdateRoomInformationFromDB()
        {
            try
            {
                var classroom = DataProvider.Ins.DB.Classrooms.Where(x => x.ClassroomID.Equals(ManageSettings.RoomID)).SingleOrDefault();
                if (classroom != null)
                {
                    AreaName = classroom.FloorOfArea.Area.AreaName;
                    FloorName = classroom.FloorOfArea.FloorName;
                    ClassroomName = classroom.ClassroomName;
                    ClassroomStatus = classroom.ClassroomStatus;
                }
                classroom.ClassroomServerIPV4Address = Server.ManageSettings.GetIPV4Server;
                classroom.ClassroomPortNumber = Server.ManageSettings.Port;
                DataProvider.Ins.DB.SaveChangesAsync();
                GenerateNotification.ShowNoti("Thông báo mới", "Đã kết nối với máy chủ.", Notifications.Wpf.NotificationType.Information);
            }
            catch(Exception ex)
            {
                GenerateNotification.ShowNoti("Thông báo mới", $"Có lỗi khi lấy dữ liệu từ server, mã lỗi: {ex.Message}", Notifications.Wpf.NotificationType.Information);
            }
        }


    }
}
