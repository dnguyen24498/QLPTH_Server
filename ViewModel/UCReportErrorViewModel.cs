using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VMHelper;
using Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace ViewModel
{
    public class UCReportErrorViewModel:BaseViewModel
    {
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        private ClassroomDevice selectedDevice;
        public ClassroomDevice SelectedDevice
        {
            get => selectedDevice;
            set
            {
                selectedDevice = value;
                OnPropertyChanged();
            }
        }

        private List<ClassroomDevice> listDevice;
        public List<ClassroomDevice> ListDevice
        {
            get => listDevice;
            set
            {
                listDevice = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Report> listReport;
        public ObservableCollection<Report> ListReport
        {
            get => listReport;
            set
            {
                listReport = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseWindowCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand ReportErrorCommand { get; set; }
        public UCReportErrorViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Grid>((p) => true, async (p) =>
              {
                  p.Visibility = System.Windows.Visibility.Visible;
                  ListReport = await GetReportAsync();
                  ListDevice = await GetDeviceAsync();
                  p.Visibility = System.Windows.Visibility.Hidden;
              });
            ReportErrorCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (SelectedDevice != null)
                {
                    Report report = new Report
                    {
                        ClassroomID = Properties.Settings.Default.ClassroomID,
                        DeviceID = SelectedDevice.DeviceID,
                        Description = Description,
                        DateCreated = DateTime.Now,
                        IsDone = false
                    };

                    if(await CreateReport(report)==true)
                    {
                        Description = "";
                        Server.Server.Instance.OnBroadcastNotification(new GeneralClass.MessageArgs("Đã thêm phiếu báo hỏng thiết bị!"));
                        ListReport = await GetReportAsync();
                    }
                    else
                    {
                        Server.Server.Instance.OnBroadcastNotification(new GeneralClass.MessageArgs("Có lỗi khi thêm dữ liệu!"));
                    }
                }
            });
            CloseWindowCommand = new RelayCommand<Window>((p) => true, async (p) =>
            {
                await Task.Run(()=> ViewModelLocator.UCMainControlViewModelStatic.GetRoomDeviceAsync());
                p.Close();
            });
        }

        private async Task<bool> CreateReport(Report report)
        {
            if(await Task.Run(() => CreateReportDetails(report)))
            {
                return true;
            }
            return false;
        }
        
        private bool CreateReportDetails(Report report)
        {
            try
            {
                var classRoomDevice = DataProvider.Ins.DB.ClassroomDevices.Where(x => x.ClassroomID == report.ClassroomID && x.DeviceID == report.DeviceID).SingleOrDefault();
                if (classRoomDevice!=null && classRoomDevice.Quantity >= 1)
                {
                    DataProvider.Ins.DB.Reports.Add(report);
                    classRoomDevice.Quantity -= 1;
                    classRoomDevice.Broken += 1;
                    DataProvider.Ins.DB.SaveChanges();
                    return true;
                }
                Server.Server.Instance.OnBroadcastNotification(new GeneralClass.MessageArgs("Có lỗi khi thêm dữ liệu!"));
                return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private async Task<List<ClassroomDevice>> GetDeviceAsync()
        {
            var result = await Task.Run(() => GetDeviceDetails());
            return result;
        }

        private List<ClassroomDevice> GetDeviceDetails()
        {
            List<ClassroomDevice> result;
            try
            {
                result = DataProvider.Ins.DB.ClassroomDevices.Where(x => x.ClassroomID == Properties.Settings.Default.ClassroomID).ToList();
                if (result.Count > 0)
                {
                    return result;
                }
                return new List<ClassroomDevice>();
            }
            catch
            {
                Server.Server.Instance.OnBroadcastNotification(new GeneralClass.MessageArgs("Có lỗi khi lấy dữ liệu!"));
                return new List<ClassroomDevice>();
            }
        }

        private async Task<ObservableCollection<Report>> GetReportAsync()
        {
                var result = await Task.Run(()=> GetReportDetails(Properties.Settings.Default.ClassroomID));
                return result;
        }

        private ObservableCollection<Report> GetReportDetails(string classroomID)
        {
            ObservableCollection<Report> result;
            try
            {
                result = new ObservableCollection<Report>(DataProvider.Ins.DB.Reports.Where(x => x.ClassroomID == Properties.Settings.Default.ClassroomID).ToList());
                if (result.Count > 0)
                {
                    return result;
                }
                return new ObservableCollection<Report>();

            }
            catch
            {
                Server.Server.Instance.OnBroadcastNotification(new GeneralClass.MessageArgs("Có lỗi khi lấy dữ liệu!"));
                return new ObservableCollection<Report>();
            }
        }

    }
}
