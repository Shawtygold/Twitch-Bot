using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;
using TwitchBot.Services;
using Wpf.Ui.Common.Interfaces;

namespace TwitchBot.MVVM.ViewModel
{
    internal class BanWordsViewModel : Core.ViewModel
    {
        public BanWordsViewModel(INavigationService navigation)
        {
            Naviagtion = navigation;

            BackCommand = new RelayCommand(Back);
            AddCommand = new RelayCommand(Add);

            StaticBanWords.CollectionChanged += StaticBanWords_CollectionChanged;

            LoadBanWords();
        }

        #region Properties

        private INavigationService _navigation;
        public INavigationService Naviagtion
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private string _text = "";
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }


        private ObservableCollection<BanWord> _banWords = new();
        public ObservableCollection<BanWord> BanWords
        {
            get { return _banWords; }
            set { _banWords = value; OnPropertyChanged(); }
        }


        private static ObservableCollection<BanWord> _staticBanWords = new();
        public static ObservableCollection<BanWord> StaticBanWords
        {
            get { return _staticBanWords; }
            set { _staticBanWords = value; }
        }


        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }
        public ICommand AddCommand { get; set; }

        private void Back(object obj) => Naviagtion.NavigateTo<StartBotViewModel>();
        private void Add(object obj)
        {
            bool isValid = DataWorker.InputValidation(Text);

            if (isValid)
            {
                //вывожу сообщение о том, было ли добавлено плохое слово
                MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.AddBanWord(new BanWord
                {
                    Text = Text

                }), msgTrue: "Плохое слово успешно добавлено!", msgFalse: "Произошла ошибка. Плохое слово не добавлено!"));

                //очищаю textbox
                Text = "";

                UpdateBanWords();
            }
            else
            {
                //вывод сообщения о нарушениях ввода данных
                MessageBox.Show("Проверьте праивльность заполнения формы! Поля не должны быть пустыми.");
            }
        }

        #endregion

        #region Events

        private void StaticBanWords_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadBanWords();
        }

        #endregion

        #region Methods

        private void LoadBanWords()
        {
            BanWords = StaticBanWords;
        }

        private void UpdateBanWords()
        {
            BanWords = DataWorker.GetBanWords();
        }

        #endregion
    }
}
