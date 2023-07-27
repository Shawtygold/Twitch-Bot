using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.Model;

namespace TwitchBot.MVVM.ViewModel.FormViewModel
{
    internal class CommandsFormViewModel : Core.ViewModel
    {
        //конструктор срабатывающий при добавлении
        public CommandsFormViewModel(string action)
        {
            Action = action;

            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);
        }

        //конструктор срабатывающий при редактировании
        public CommandsFormViewModel(string action, Command command)
        {
            Action = action;

            CloseCommand = new RelayCommand(Close);
            MinimizeCommand = new RelayCommand(Minimize);
            AcceptCommand = new RelayCommand(Accept);

            if(command != null)
            {
                //заполнение значений
                Id = command.Id;
                Title = command.Title;
                ResponceType = command.ResponceType;
                IsEnabled = command.IsEnabled;
            }       
        }

        #region Properties

        public string AppTitle { get; set; } = "TwitchBot";
        public string Action { get; set; } = "";


        public int Id { get; set; }
        public bool IsEnabled { get; set; }


        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }


        private string _responceType = "";
        public string ResponceType
        {
            get { return _responceType; }
            set { _responceType = value; OnPropertyChanged(); }
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
            if(obj is Window form)
            {
                form.WindowState = WindowState.Minimized;
            }
        }
        private void Accept(object obj)
        {
            if(obj is Window form)
            {
                bool isValid = DataWorker.InputValidation(Title, ResponceType);

                if (isValid)
                {
                    if(Action == "Добавление")
                    {
                        //вывод сообщения о том была ли добавлена команда
                        MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.AddCommand(new Command()
                        {
                            Title = Title,
                            ResponceType = ResponceType,
                            IsEnabled = true

                        }), msgTrue: "Команда успешно добавлена!", msgFalse:"Произошла ошибка. Команда не добавлена!"));
                    }
                    else if(Action == "Редактирование")
                    {
                        //вывод сообщения о том была ли отредактирована команда
                        MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.EditCommand(new Command
                        {
                            Id = Id,
                            Title = Title,
                            ResponceType = ResponceType,
                            IsEnabled = IsEnabled

                        }), msgTrue: "Команда успешно отредактирована!", msgFalse: "Произошла ошибка. Команда не была отредактирована!"));
                    }

                    //закрытие формы 
                    form.Close();
                }
                else
                {
                    //вывод сообщения о нарушениях ввода данных
                    MessageBox.Show("Проверьте праивльность заполнения формы! Поля не должны быть пустыми.");
                }
            }            
        }

        #endregion
    }
}
