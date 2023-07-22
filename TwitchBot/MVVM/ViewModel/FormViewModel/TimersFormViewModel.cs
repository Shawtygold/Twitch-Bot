using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;

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
                IsEnabled = timer.IsEnabled;
                AutoReset = timer.AutoReset;
                MessageInterval = timer.MessageInterval;
            }
        }

        #region Properties

        public string AppTitle { get; set; } = "TwitchBot";
        public string Action { get; set; } = "";


        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public bool AutoReset { get; set; } = false;


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
            set { _interval = value; OnPropertyChanged(); }
        }


        private int _messageInterval = 0;
        public int MessageInterval
        {
            get { return _messageInterval; }
            set { _messageInterval = value; OnPropertyChanged(); }
        }


        private string _responceMessage = "";
        public string ResponceMessage
        {
            get { return _responceMessage; }
            set { _responceMessage = value; OnPropertyChanged(); }
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
                string message = DataWorker.InputValidation(Title, ResponceMessage, Interval, MessageInterval);

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
                            IsEnabled = true,
                            AutoReset = AutoReset,
                            MessageInterval = MessageInterval

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
                            IsEnabled = IsEnabled,
                            AutoReset = AutoReset,
                            MessageInterval = MessageInterval

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
