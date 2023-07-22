using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Input;
using TwitchBot.Core;

namespace TwitchBot.MVVM.Model
{
    internal class Command
    {
        public Command()
        {
            CheckedCommand = new RelayCommand(Checked);
            UncheckedCommand = new RelayCommand(Unchecked);
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ResponceType { get; set; } = null!; //сообщение, которое выводит бот при вводе пользователем команды в чат
        public bool IsEnabled { get; set; } = false;

        #region Commands

        [NotMapped] public ICommand CheckedCommand { get; set; }
        [NotMapped] public ICommand UncheckedCommand { get; set; }

        private void Checked(object obj)
        {
            //изменяю значение IsActive в базе данных на true
            DataWorker.ChangeIsEnabledInCommand(Id, true);
        }
        private void Unchecked(object obj)
        {
            //изменяю значение IsActive в базе данных на true
            DataWorker.ChangeIsEnabledInCommand(Id, false);
        }

        #endregion
    }
}
