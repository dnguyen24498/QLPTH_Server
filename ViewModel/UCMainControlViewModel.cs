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
            DisableControl = true;
            SelectedView = 0;
            LoadedUCMainControl = new RelayCommand<object>((p) => true, async (p) =>
              {
                  await getRoomDeviceAsync();
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
                  }
                  else if (SelectedView == 1)
                  {
                      p.ItemsSource = _systemNotification;
                  }
                  else if (SelectedView == 2)
                  {
                      p.ItemsSource = _userNotification;
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


        private async Task<List<SettingProcess>> GetSettingProcessArgs(string processName)
        {
            var result = await Task.Run(() => DataProvider.Ins.DB.SettingProcesses.Where(x => x.ProcessName.Contains(processName.ToLower())).ToList()) ?? null;
            if (result.Count > 0)
            {
                return result;
            }
            return null;
        }

        private async Task getRoomDeviceAsync()
        {
            ListRoomDevice = await Task.Run(() => new ObservableCollection<ClassroomDevice>(DataProvider.Ins.DB.ClassroomDevices.Where(x => x.ClassroomID.Equals(ManageSettings.RoomID)).ToList()));
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
