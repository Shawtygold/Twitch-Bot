﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    internal class StartBotViewModel : Core.ViewModel
    {
        public StartBotViewModel(INavigationService navigation)
        {
            Navigation = navigation;
        }

        #region Properties

        #region Navigation

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }

        #endregion

        #region IndicatorBackground

        private string _indicatorBackground = "#414141";
        public string IndicatorBackground
        {
            get { return _indicatorBackground; }
            set { _indicatorBackground = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion
    }
}
