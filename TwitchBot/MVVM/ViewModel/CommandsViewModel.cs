using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;
using TwitchBot.MVVM.View.FormView;
using TwitchBot.MVVM.ViewModel.FormViewModel;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    internal class CommandsViewModel : Core.ViewModel
    {
        public CommandsViewModel(INavigationService naviagtion)
        {
            Navigation = naviagtion;

            BackCommand = new RelayCommand(Back);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);

            LoadCommand();
        }

        #region Properties

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private static ObservableCollection<Command> _staticCommands = new();
        public static ObservableCollection<Command> StaticCommands
        {
            get { return _staticCommands; }
            set { _staticCommands = value; }
        }


        private ObservableCollection<Command> _commands = new();
        public ObservableCollection<Command> Commands
        {
            get { return _commands; }
            set { _commands = value; OnPropertyChanged(); }
        }


        private Command _selectedItem;
        public Command SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private void Back(object obj) => Navigation.NavigateTo<StartBotViewModel>();
        private void Add(object obj)
        {
            CommandsForm form = new();
            form.DataContext = new CommandsFormViewModel("Добавление");
            form.ShowDialog();

            //обновление списка команд
            UpdateCommands();
        }
        private void Edit(object obj)
        {
            if (SelectedItem != null)
            {
                CommandsForm form = new();
                form.DataContext = new CommandsFormViewModel("Редактирование", SelectedItem);
                form.ShowDialog();

                UpdateCommands();
            }
            else
            {
                MessageBox.Show("Выберите команду для редактирования!");
            }
        }
        private void Delete(object obj)
        {
            if(SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить эту команду?", caption:" ", button: MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                {
                    //вывод сообщения о результатах удаления
                    MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.DeleteCommand(SelectedItem),
                        "Команда успешно удалена!", 
                        "Произошла ошибка. Команда не была удалена!"));
                }

                //обновление списка команд
                UpdateCommands();
            }
            else
            {
                MessageBox.Show("Выберите команду для удаления!");
            }
        }

        #endregion

        #region Methods

        //передача списка команд из StaticCommands в Commands
        private void LoadCommand()
        {
            Commands = StaticCommands;           
        }

        //обновление списка команд
        private void UpdateCommands()
        {
            Commands = DataWorker.GetCommands();
        }

        #endregion
    }
}
