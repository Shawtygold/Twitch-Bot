using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.View.FormView;
using TwitchBot.MVVM.ViewModel;
using TwitchBot.MVVM.ViewModel.FormViewModel;

namespace TwitchBot.MVVM.Model
{
    class Timer
    {
        public Timer()
        {
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ResponceMessage { get; set; } = null!;
        public int Interval { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]public ICommand EditCommand { get; set; }
        private void Edit(object obj)
        {
            TimersForm form = new();
            form.DataContext = new TimersFormViewModel("Редактирование", this);
            form.ShowDialog();
        }


        [NotMapped] public ICommand DeleteCommand { get; set; }
        private void Delete(object obj)
        {
            MessageBox.Show(DataWorker.GetMessageAboutAction(DataWorker.DeleteTimer(this), "Таймер успешно удален!", "Произошла ошибка. Таймер не был удален!"));
        }
    }
}
