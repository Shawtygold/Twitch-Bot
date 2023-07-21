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
            NaviagteToCommandsCommand = new RelayCommand(NavigateToCommands);
            NaviagteToTimersCommand = new RelayCommand(NavigateToTimers);
        }

        //бот
        ShawtygoldqBot bot = new();

        #region Properties

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        //индикатор на кнопке
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
        public ICommand NaviagteToTimersCommand { get; set; }

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
        private void NavigateToCommands(object obj)
        {
            //экран загрузки
            Navigation.NavigateTo<LoadingScreenViewModel>();

            //получение списка команд из базы данных в фоновом потоке
            Thread thread = new(GetCommands);
            thread.Start();        
        }
        private void NavigateToTimers(object obj)
        {
            //экран загрузки
            Navigation.NavigateTo<LoadingScreenViewModel>();

            //получение таймеров из базы данных в фоновом потоке
            Thread thread = new(GetTimers);
            thread.Start();
        }


        private void GetCommands()
        {
            //получение списка команд
            CommandsViewModel.StaticCommands = DataWorker.GetCommands();

            Navigation.NavigateTo<CommandsViewModel>();
        }
        private void GetTimers()
        {
            //получение таймеров
            TimersViewModel.StaticTimers = DataWorker.GetTimers();

            Navigation.NavigateTo<TimersViewModel>();
        }

        #endregion
    }
}
