﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                IsActive = command.IsActive;
            }       
        }

        #region Properties

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string AppTitle { get; set; } = "TwitchBot";


        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }


        private string _responceType;
        public string ResponceType
        {
            get { return _responceType; }
            set { _responceType = value; OnPropertyChanged(); }
        }


        private string _action;
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
            if(obj is Window form)
            {
                form.WindowState = WindowState.Minimized;
            }
        }
        private void Accept(object obj)
        {
            if(obj is Window form)
            {
                //получение сообщения о правильности ввода данных
                string message = DataWorker.InputValidation(Title, ResponceType);

                if (message == "Ok")
                {
                    if(Action == "Добавление")
                    {
                        //вывод сообщения о том была ли добавлена команда
                        MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.AddCommand(new Command()
                        {
                            Title = Title,
                            ResponceType = ResponceType,
                            IsActive = true

                        }), msgTrue:"Команда успешно добавлена", msgFalse:"Произошла ошибка. Команда не добавлена!"));
                    }
                    else if(Action == "Редактирование")
                    {
                        //вывод сообщения о том была ли отредактирована команда
                        MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.EditCommand(new Command
                        {
                            Id = Id,
                            Title = Title,
                            ResponceType = ResponceType,
                            IsActive = IsActive

                        }), msgTrue: "Команда успешно отредактирована", msgFalse: "Произошла ошибка. Команда не была отредактирована!"));
                    }

                    //закрытие формы 
                    form.Close();
                }
                else
                {
                    MessageBox.Show(message);
                }
            }            
        }

        #endregion
    }
}