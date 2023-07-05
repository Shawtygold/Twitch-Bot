using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    internal class StartBotViewModel : Core.ViewModel
    {
        public StartBotViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            StartBotCommand = new RelayCommand(StartBot);
        }

        //инициализирую бота
        ShawtygoldqBot bot = new();

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

        #region Commands

        public ICommand StartBotCommand { get; set; }
        private void StartBot(object obj)
        {            
            if (bot.BotStatus == "Off")
            {
                IndicatorBackground = "#4CCD68";

                bot.Connect();
            }
            else if (bot.BotStatus == "On")
            {
                IndicatorBackground = "#414141";

                bot.Disconnect();
            }
        }

        #endregion
    }
}
