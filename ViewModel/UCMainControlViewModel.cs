using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using GeneralClass;
using Server;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Model;
namespace ViewModel
{
    public class UCMainControlViewModel : BaseViewModel
    {
        private ObservableCollection<MessageArgs> _systemNotification = new ObservableCollection<MessageArgs>();
        private ObservableCollection<MessageArgs> _userNotification = new ObservableCollection<MessageArgs>();
        private ObservableCollection<ClassroomDevice> _listRoomDevice;
        public ObservableCollection<ClassroomDevice> ListRoomDevice
        {
            get => _listRoomDevice;
            set
            {
                _listRoomDevice = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadedUCMainControl { get; set; }

        private int numbNotification;
        public int NumbNotification
        {
            get => numbNotification;
            set
            {
                numbNotification = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddProcessCommand { get; set; }

        public ICommand DeleteProcessCommand { get; set; }

        public ICommand ActionControlCommand { get; set; }

        public ICommand PreviewKeyDownCommand { get; set; }

        public ICommand ChangeViewNotificationCommand { get; set; }

        private bool _openOnlyProcess;
        public bool OpenOnlyProcess
        {
            get => _openOnlyProcess;
            set
            {
                _openOnlyProcess = value;
                OnPropertyChanged();
            }
        }

        private int _selectedView;
        public int SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
            }
        }

        private bool _prohibitProcess;
        public bool ProhibitProcess
        {
            get => _prohibitProcess;
            set
            {
                _prohibitProcess = value;
                OnPropertyChanged();
            }
        }

        private bool _disableControl;
        public bool DisableControl
        {
            get => _disableControl;
            set
            {
                _disableControl = value;
                OnPropertyChanged();
            }
        }

        private SettingProcessArgs _selectedProcess;
        public SettingProcessArgs SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SettingProcessArgs> _listProcess;
        public ObservableCollection<SettingProcessArgs> ListProcess
        {
            get => _listProcess;
            set
            {
                _listProcess = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MessageArgs> _listNotification;
        public ObservableCollection<MessageArgs> ListNotification
        {
            get => _listNotification;
            set
            {
                _listNotification = value;
                OnPropertyChanged();
            }
        }

        public UCMainControlViewModel()
        {
            ListNotification = new ObservableCollection<MessageArgs>();
            NumbNotification = ListNotification.Count();
            DisableControl = true;
            SelectedView = 0;
            LoadedUCMainControl = new RelayCommand<object>((p) => true,(p) =>
              {
                  GetRoomDeviceAsync();
                  Server.Server.Instance.BroadcastNotification += Instance_BroadcastNotification;
                  ListProcess = Server.ManageSettings.GetListSettingProcess();
              });
            AddProcessCommand = new RelayCommand<ComboBox>((p) => true, (p) =>
              {
                  if (p.SelectedIndex != -1)
                  {
                      SettingProcess settingProcess = p.SelectedItem as SettingProcess;
                      SettingProcessArgs settingProcessArgs = new SettingProcessArgs
                      {
                          ProcessName = settingProcess.ProcessName,
                          ProcessIcon = settingProcess.ProcessIcon,
                          IconForeground = settingProcess.IconForeground
                      };
                      Server.ManageSettings.AddSettingProcess(settingProcessArgs);
                      ListProcess = Server.ManageSettings.GetListSettingProcess();
                  }
              });
            DeleteProcessCommand = new RelayCommand<object>((p) => true, (p) =>
              {
                  if (SelectedProcess != null)
                  {
                      Server.ManageSettings.RemoveSettingProcess(SelectedProcess);
                      ListProcess = Server.ManageSettings.GetListSettingProcess();
                  }

              });
            ActionControlCommand = new RelayCommand<TextBox>((p) => true, (p) =>
              {
                  if (ProhibitProcess)
                  {
                      Server.Server.Instance.ProhibitOpenProcess(getInterval(p.Text));
                  }
                  else if (OpenOnlyProcess)
                  {
                      Server.Server.Instance.OpenOnlyProcess(getInterval(p.Text));
                  }
                  else if (DisableControl)
                  {
                      Server.Server.Instance.DisableControlAllClient();
                  }
              });
            ChangeViewNotificationCommand = new RelayCommand<ListBox>((p) => true, (p) =>
              {
                  if (SelectedView == 0)
                  {
                      p.ItemsSource = ListNotification;
                      NumbNotification = ListNotification.Count;
                  }
                  else if (SelectedView == 1)
                  {
                      p.ItemsSource = _systemNotification;
                      NumbNotification = _systemNotification.Count;
                  }
                  else if (SelectedView == 2)
                  {
                      p.ItemsSource = _userNotification;
                      NumbNotification = _userNotification.Count;
                  }
              });

            PreviewKeyDownCommand = new RelayCommand<ComboBox>((p) =>
            {
                if (!string.IsNullOrEmpty(p.Text))
                {
                    return true;
                }
                p.ItemsSource = null;
                p.IsDropDownOpen = false;
                return false;
            }, async (p) =>
              {
                  List<SettingProcess> settingProcesses = await GetSettingProcessArgs(p.Text);
                  if (settingProcesses != null)
                  {
                      p.ItemsSource = settingProcesses;
                      p.IsDropDownOpen = true;
                      return;
                  }
                  p.ItemsSource = null;
                  p.IsDropDownOpen = false;

              });
        }

        public async void GetRoomDeviceAsync()
        {
            await Task.Run(() => getRoomDeviceAsync());
        }

        private async Task<List<SettingProcess>> GetSettingProcessArgs(string processName)
        {
            try
            {
                var result = await Task.Run(()=>getSettingProcess(processName));
                if (result.Count > 0)
                {
                    return result;
                }
            }
            catch
            {
                Server.Server.Instance.OnBroadcastNotification(new MessageArgs("Server database không phản hồi"));
                return null;
            }
            return null;
        }
        
        private List<SettingProcess> getSettingProcess(string processName)
        {
            try
            {
                return DataProvider.Ins.DB.SettingProcesses.Where(x => x.ProcessName.Contains(processName.ToLower())).ToList();
            }
            catch
            {
                return new List<SettingProcess>();
            }
        }

        private void getRoomDeviceAsync()
        {
            try
            {
                ListRoomDevice = new ObservableCollection<ClassroomDevice>(DataProvider.Ins.DB.ClassroomDevices.Where(x => x.ClassroomID.Equals(ManageSettings.RoomID)).ToList());
            }
            catch
            {
                ListRoomDevice = new ObservableCollection<ClassroomDevice>();
                Server.Server.Instance.OnBroadcastNotification(new MessageArgs("Server database không phản hồi"));
            }
        }

        private int getInterval(string txt)
        {
            if (!string.IsNullOrEmpty(txt))
            {
                try
                {
                    int interval = int.Parse(txt) * 1000;
                    return interval;
                }
                catch
                {
                    return 2000;
                }
            }
            return 2000;

        }
        private void Instance_BroadcastNotification(object sender, MessageArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListNotification.Add(e);
                NumbNotification++;
                if (e.Type == "InformationCircle")
                {
                    _systemNotification.Add(e);
                }
                else if (e.Type == "UserSupervisorCircle")
                {
                    _userNotification.Add(e);
                }
            });

        }
    }
}
