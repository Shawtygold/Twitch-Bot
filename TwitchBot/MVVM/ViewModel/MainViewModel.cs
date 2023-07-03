using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    class MainViewModel : Core.ViewModel
    {
        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;
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
    }
}
