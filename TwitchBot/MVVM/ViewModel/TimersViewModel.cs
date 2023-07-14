﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    class TimersViewModel : Core.ViewModel
    {
        public TimersViewModel(INavigationService navigation)
        {
            Navigation = navigation;
        }

        #region Properties

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        #endregion
    }
}
