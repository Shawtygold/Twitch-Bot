using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    internal class CommandsViewModel : Core.ViewModel
    {
        public CommandsViewModel(INavigationService naviagtion)
        {
            Navigation = naviagtion;

            BackCommand = new RelayCommand(Back);

            //загрузка команд
            LoadCommand();
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

        #region StaticCommands

        private static List<Command> _staticCommands;
        public static List<Command> StaticCommands
        {
            get { return _staticCommands; }
            set { _staticCommands = value; }
        }

        #endregion

        #region Commands

        private List<Command> _commands;
        public List<Command> Commands
        {
            get { return _commands; }
            set { _commands = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        private void Back(object obj) => Navigation.NavigateTo<StartBotViewModel>();

        #endregion

        #region Methods

        //передача списка команд из StaticCommands в Commands
        private void LoadCommand()
        {
            if(StaticCommands != null)
            {
                Commands = StaticCommands;
            }
        }

        #endregion
    }
}
