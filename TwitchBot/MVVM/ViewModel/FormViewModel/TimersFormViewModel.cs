using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TwitchBot.MVVM.Model;
using TwitchBot.Core;

namespace TwitchBot.MVVM.ViewModel.FormViewModel
{
    class TimersFormViewModel : Core.ViewModel
    {
        public TimersFormViewModel(string action)
        {
            Action = action;

            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);
        }

        public TimersFormViewModel(string action, Timer timer)
        {
            Action = action;

            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);

            if (timer != null)
            {
                Id = timer.Id;
                Title = timer.Title;
                Interval = timer.Interval;
                ResponceMessage = timer.ResponceMessage;
                IsActive = timer.IsActive;
            }
        }

        #region Properties

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string AppTitle { get; set; } = "TwitchBot";


        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }


        private int _interval = 0;
        public int Interval
        {
            get { return _interval; }
            set {  _interval = value;  OnPropertyChanged(); }
        }


        private string _responceMessage = "";
        public string ResponceMessage
        {
            get { return _responceMessage; }
            set { _responceMessage = value; OnPropertyChanged(); }
        }


        private string _action = "";
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        #endregion

        #region Commands

        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand AcceptCommand { get; set; }

        private void Close(object obj)
        {
            if (obj is Window form)
            {
                form.Close();
            }
        }
        private void Minimize(object obj)
        {
            if (obj is Window form)
            {
                form.WindowState = WindowState.Minimized;
            }
        }
        private void Accept(object obj)
        {
            if (obj is Window form)
            {
                //получение сообщения о правильности ввода данных
                string message = DataWorker.InputValidation(Title, ResponceMessage, Interval);

                if (message == "Ok")
                {
                    if (Action == "Добавление")
                    {
                        //вывод сообщения о том была ли добавлена команда
                        MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.AddTimer(new Timer()
                        {
                            Title = Title,
                            ResponceMessage = ResponceMessage,
                            Interval = Interval,
                            IsActive = true

                        }), msgTrue: "Таймер успешно добавлен!", msgFalse: "Произошла ошибка. Таймер не добавлен!"));
                    }
                    else if (Action == "Редактирование")
                    {
                        //вывод сообщения о том была ли отредактирована команда
                        MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.EditTimer(new Timer
                        {
                            Id = Id,
                            Title = Title,
                            ResponceMessage = ResponceMessage,
                            Interval = Interval,
                            IsActive = IsActive

                        }), msgTrue: "Таймер успешно отредактирован!", msgFalse: "Произошла ошибка. Таймер не был отредактирован!"));
                    }

                    //закрытие формы 
                    form.Close();
                }
                else
                {
                    //вывод сообщения о нарушениях ввода данных
                    MessageBox.Show(message);
                }
            }
            else
            {
                MessageBox.Show("Alo!");
            }
        }

        #endregion
    }
}
