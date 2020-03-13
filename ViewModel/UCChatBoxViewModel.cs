using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows;
using GeneralClass;
namespace ViewModel
{
    public class UCChatBoxViewModel:BaseViewModel
    {
        public ICommand SendMessageCommand { get; set; }
        public ICommand SendFileCommand { get; set; }
        private ObservableCollection<MessageArgs> _converstation;
        public ObservableCollection<MessageArgs> Converstation
        {
            get => _converstation;
            set
            {
                _converstation = value;
                OnPropertyChanged();
            }
        }
        public UCChatBoxViewModel()
        {
            Server.Server.Instance.BroadcastMessage += Instance_BroadcastMessage;
            Converstation = new ObservableCollection<MessageArgs>();
            SendMessageCommand = new RelayCommand<TextBox>((p) =>
            {
                if (!string.IsNullOrEmpty(p.Text) && !string.IsNullOrWhiteSpace(p.Text))
                {
                    return true;
                }
                return false;
            }, (p) =>
            {
                Server.Server.Instance.SendMessage(p.Text);
                Converstation.Add(new MessageArgs(p.Text));
                p.Clear();
                p.Focus();
            });
            SendFileCommand = new RelayCommand<object>((p) => true, (p) =>
              {
                  OpenFileDialog dia = new OpenFileDialog();
                  dia.RestoreDirectory = true;
                  dia.Title = "Chọn tập tin đính kèm";
                  if (dia.ShowDialog() == true)
                      Server.Server.Instance.SendFile(dia.FileName);
              });
        }

        private void Instance_BroadcastMessage(object sender, MessageArgs e)
        {
            e.Type = "UserSupervisorCircle";
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Converstation.Add(e);
            });
        }
    }
}
