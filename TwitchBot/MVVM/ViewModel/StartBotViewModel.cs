using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
            NaviagteToCommandsCommand = new RelayCommand(NaviagteToCommands);
        }

        //инициализирую бота
        ShawtygoldqBot bot = new();

        #region Properties

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private string _indicatorBackground = "#414141";
        public string IndicatorBackground
        {
            get { return _indicatorBackground; }
            set { _indicatorBackground = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand StartBotCommand { get; set; }
        public ICommand NaviagteToCommandsCommand { get; set; }

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
        private void NaviagteToCommands(object obj)
        {
            //экран загрузки
            Navigation.NavigateTo<LoadingScreenViewModel>();

            //получение списка команд из базы данных в фоновом потоке
            Thread thread = new(GetData);
            thread.Start();        
        }

        private void GetData()
        {
            CommandsViewModel.StaticCommands = DataWorker.GetCommands();
            Navigation.NavigateTo<CommandsViewModel>();
        }

        #endregion
    }
}
