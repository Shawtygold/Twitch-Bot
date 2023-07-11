using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.MVVM.Model;

namespace TwitchBot.MVVM.ViewModel.FormViewModel
{
    internal class CommandsFormViewModel : Core.ViewModel
    {
        public CommandsFormViewModel(string action)
        {
            Action = action;
        }

        public CommandsFormViewModel(string action, Command command)
        {
            Action = action;

            Id = command.Id;
            Title = command.Title;
            ResponceType = command.ResponceType;
            isActive = command.IsActive;
        }

        #region Properties

        public int Id { get; set; }
        public bool isActive { get; set; }

        private string _title;
        private string _responceType;
        private string _action;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        public string ResponceType
        {
            get { return _responceType; }
            set { _responceType = value; OnPropertyChanged(); }
        }
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }


        #endregion
    }
}
