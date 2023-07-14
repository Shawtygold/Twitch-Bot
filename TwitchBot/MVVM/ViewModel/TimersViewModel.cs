using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;
using TwitchBot.MVVM.View.FormView;
using TwitchBot.MVVM.ViewModel.FormViewModel;
using TwitchBot.Services;

namespace TwitchBot.MVVM.ViewModel
{
    class TimersViewModel : Core.ViewModel
    {
        public TimersViewModel(INavigationService navigation)
        {
            Navigation = navigation;

            BackCommand = new RelayCommand(Back);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);

            //загрузка таймеров из StaticTimers в Timers
            LoadTimers();
        }

        #region Properties

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private Timer _selectedItem;
        public Timer SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }


        private static ObservableCollection<Timer> _staticTimers = new();
        public static ObservableCollection<Timer> StaticTimers
        {
            get { return _staticTimers; }
            set { _staticTimers = value; }
        }


        private ObservableCollection<Timer> _timers = new();
        public ObservableCollection<Timer> Timers
        {
            get { return _timers; }
            set { _timers = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commads

        public ICommand BackCommand{ get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private void Back(object obj) => Navigation.NavigateTo<StartBotViewModel>();
        private void Add(object obj)
        {
            TimersForm form = new();
            form.DataContext = new TimersFormViewModel("Добавление");
            form.ShowDialog();

            //обновление списка таймеров
            UpdateTimers();
        }
        private void Edit(object obj)
        {
            if(SelectedItem != null)
            {
                TimersForm form = new();
                form.DataContext = new TimersFormViewModel("Редактирование", SelectedItem);
                form.ShowDialog();

                //обновление списка таймеров
                UpdateTimers();
            }
            else
            {
                MessageBox.Show("Выберите таймер!");
            }
        }
        private void Delete(object obj)
        {
            if (SelectedItem != null) 
            {
                MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.DeleteTimer(SelectedItem), "Таймер успешно удален!", "Произошла ошибка. Таймер не был удален!"));

                //обновление списка таймеров
                UpdateTimers();
            }
            else
            {
                MessageBox.Show("Выберите таймер!");
            }
        }

        #endregion

        #region Methods

        private void LoadTimers()
        {
            Timers = StaticTimers;
        }
        private void UpdateTimers()
        {
            //получение таймеров
            Timers = DataWorker.GetTimers();
        }

        #endregion
    }
}
