using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ViewModel
{
    public class ViewModelLocator
    {
        private static MainWindowViewModel _mainWindowViewModelStatic;
        private static UCClientInforViewModel _ucClientInforViewModelStatic;
        private static UCChatBoxViewModel _uCChatBoxViewModelStatic;
        private static UCMainControlViewModel _uCMainControlViewModelStatic;
        private static SettingWindowViewModel _settingWindowViewModelStatic;
        public static SettingWindowViewModel SettingWindowViewModelStatic
        {
            get
            {
                if (_settingWindowViewModelStatic == null)
                {
                    _settingWindowViewModelStatic = new SettingWindowViewModel();
                }
                return _settingWindowViewModelStatic;
            }
        }
        public static UCMainControlViewModel UCMainControlViewModelStatic
        {
            get
            {
                if (_uCMainControlViewModelStatic == null)
                {
                    _uCMainControlViewModelStatic = new UCMainControlViewModel();
                }
                return _uCMainControlViewModelStatic;
            }
        }
        public static UCChatBoxViewModel UCChatBoxViewModelStatic 
        {
            get
            {
                if (_uCChatBoxViewModelStatic == null)
                    _uCChatBoxViewModelStatic = new UCChatBoxViewModel();
                
                return _uCChatBoxViewModelStatic;
            }
        }
        public static UCClientInforViewModel UCClientInforViewModelStatic
        {
            get
            {
                if (_ucClientInforViewModelStatic == null)
                    _ucClientInforViewModelStatic = new UCClientInforViewModel();
                return _ucClientInforViewModelStatic;
            }
        }
        public static MainWindowViewModel MainWindowViewModelStatic
        {
            get
            {
                if (_mainWindowViewModelStatic == null)
                {
                    _mainWindowViewModelStatic = new MainWindowViewModel();
                }
                return _mainWindowViewModelStatic;
            }
        }
    }
}
