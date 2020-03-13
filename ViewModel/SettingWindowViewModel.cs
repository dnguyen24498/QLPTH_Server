using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using Server;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;
using TextBox = System.Windows.Controls.TextBox;

namespace ViewModel
{
    public class SettingWindowViewModel:BaseViewModel
    {
        public ICommand ChoosePathSaveFileCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand SaveSettingsCommand { get; set; }

        private string _classroomID;
        public string ClassroomID
        {
            get => _classroomID;
            set
            {
                _classroomID = value;
                OnPropertyChanged();
            }
        }

        private int _port;
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private bool _notificationSound;
        public bool NotificationSound
        {
            get => _notificationSound;
            set
            {
                _notificationSound = value;
                OnPropertyChanged();
            }
        }

        private bool _notification;
        public bool Notification
        {
            get => _notification;
            set
            {
                _notification = value;
                OnPropertyChanged();
            }
        }

        private string _pathSaveFile;
        public string PathSaveFile
        {
            get => _pathSaveFile;
            set
            {
                _pathSaveFile = value;
                OnPropertyChanged();
            }
        }

        private float _maxFileSize;
        public float MaxFileSize
        {
            get => _maxFileSize;
            set
            {
                _maxFileSize = value;
                OnPropertyChanged();
            }
        }

        private bool _acceptReceiveFile;
        public bool AcceptReceiveFile
        {
            get => _acceptReceiveFile;
            set
            {
                _acceptReceiveFile = value;
                OnPropertyChanged();
            }
        }

        private bool _deleteAllFileWhenClose;
        public bool DeleteAllFileWhenClose
        {
            get => _deleteAllFileWhenClose;
            set
            {
                _deleteAllFileWhenClose = value;
                OnPropertyChanged();
            }
        }

        private string _ipAddress;
        public string IPAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged();
            }
        }
        public SettingWindowViewModel()
        {
            LoadedWindowCommand = new RelayCommand<object>((p) => true, (p) =>
              {
                  GetSettings();
              });
            ChoosePathSaveFileCommand = new RelayCommand<TextBox>((p) => true, (p) => {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        PathSaveFile = fbd.SelectedPath;
                    }
                }
            });
            SaveSettingsCommand = new RelayCommand<Window>((p) => true, (p) =>
              {
                  UpdateNewSettings();
                  p.Close();
              });
        }

        private void UpdateNewSettings()
        {
            Server.ManageSettings.PathSaveFile = PathSaveFile;
            Server.ManageSettings.MaxFileSize = MaxFileSize;
            Server.ManageSettings.AcceptReceiveFile = AcceptReceiveFile;
            Server.ManageSettings.DeleteAllFileWhenClose = DeleteAllFileWhenClose;

            ManageSettings.Notification = Notification;
            ManageSettings.NotificationSound = NotificationSound;
        }

        private void GetSettings()
        {
            Port = Server.ManageSettings.Port;
            PathSaveFile = Server.ManageSettings.PathSaveFile;
            MaxFileSize = Server.ManageSettings.MaxFileSize;
            AcceptReceiveFile = Server.ManageSettings.AcceptReceiveFile;
            DeleteAllFileWhenClose = Server.ManageSettings.DeleteAllFileWhenClose;
            IPAddress = Server.ManageSettings.GetIPV4Server;
            ClassroomID = ManageSettings.RoomID;

            Notification = ManageSettings.Notification;
            NotificationSound = ManageSettings.NotificationSound;

        }
    }
}
