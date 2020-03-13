using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMHelper;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using Server;
using System.Collections;

namespace ViewModel
{
    public class UCClientInforViewModel : BaseViewModel
    {
        private int _numbClientOnline;
        public int NumbClientOnline
        {
            get => _numbClientOnline;
            set
            {
                _numbClientOnline = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndexCombobox;
        public int SelectedIndexCombobox
        {
            get => _selectedIndexCombobox;
            set
            {
                _selectedIndexCombobox = value;
                OnPropertyChanged();
            }
        }

        private int _totalClient;
        public int TotalClient
        {
            get => _totalClient;
            set
            {
                _totalClient = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedUserControlCommand { get; set; }
        public ICommand ActionCommand { get; set; }

        private ObservableCollection<Client> _listClient;
        public ObservableCollection<Client> ListClient
        {
            get => _listClient;
            set
            {
                _listClient = value;
                OnPropertyChanged();
            }
        }

        public UCClientInforViewModel()
        {
            NumbClientOnline = 0;
            SelectedIndexCombobox = 0;
            Server.Server.Instance.ClientConnect += Instance_ClientConnect;
            Server.Server.Instance.ClientDisconnect += Instance_ClientDisconnect;
            ListClient = new ObservableCollection<Client>(Server.Server.Instance.ListClient);
            ActionCommand = new RelayCommand<ListBox>((p)=>true, (p) =>
              {
                  if (p.SelectedItems.Count > 0)
                  {
                      HandleUserSelectionForSomeClient(p.SelectedItems);
                  }
                  else
                  {
                      HandleUserSelectionForAllClient();
                  }
              });


    }

        private void HandleUserSelectionForSomeClient(IList selectedItems)
        {
            if (SelectedIndexCombobox == 0)
            {
                Server.Server.Instance.Shutdown(selectedItems as List<Client>);
            }
            else if (SelectedIndexCombobox == 1)
            {
                Server.Server.Instance.Restart(selectedItems as List<Client>);
            }
            else if (SelectedIndexCombobox == 2)
            {
                Server.Server.Instance.Sleep(selectedItems as List<Client>);
            }
            else if (SelectedIndexCombobox == 3)
            {
                Server.Server.Instance.Hibernate(selectedItems as List<Client>);
            }
            else if (SelectedIndexCombobox == 4)
            {
                Server.Server.Instance.Lock(selectedItems as List<Client>);
            }
            else if (SelectedIndexCombobox == 5)
            {
                Server.Server.Instance.Logoff(selectedItems as List<Client>);
            }
            else if (SelectedIndexCombobox == 6 && selectedItems.Count==1)
            {
                Server.Server.Instance.CaptureScreen(selectedItems[0] as Client);
            }

        }

        private void HandleUserSelectionForAllClient()
        {
            if (SelectedIndexCombobox == 0)
            {
                Server.Server.Instance.ShutdownAll();
            }
            else if (SelectedIndexCombobox == 1)
            {
                Server.Server.Instance.RestartAll();
            }
            else if (SelectedIndexCombobox == 2)
            {
                Server.Server.Instance.SleepAll();
            }
            else if (SelectedIndexCombobox == 3)
            {
                Server.Server.Instance.HibernateAll();
            }
            else if (SelectedIndexCombobox == 4)
            {
                Server.Server.Instance.LockAll();
            }
            else if (SelectedIndexCombobox == 5)
            {
                Server.Server.Instance.LogoffAll();
            }
        }

        private void Instance_ClientDisconnect(object sender, Client e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListClient.Remove(e);
                NumbClientOnline = Server.Server.Instance.ListClient.Count();
            });
        }

        private void Instance_ClientConnect(object sender, Client e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListClient.Add(e);
                NumbClientOnline = Server.Server.Instance.ListClient.Count();
            });
        }
        
    }
}
