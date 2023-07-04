using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TwitchBot.Core;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    class MainViewModel : Core.ViewModel
    {
        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
        }

        #region Properties

        #region Naviagtion

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        #endregion

        #region AppTitle

        private string _appTitle = "TwitchBot";
        public string AppTitle
        {
            get { return _appTitle; }
            set { _appTitle = value; }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }

        private void Close(object obj) => Application.Current.Shutdown();
        private void Minimize(object obj) => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        #endregion
    }
}
