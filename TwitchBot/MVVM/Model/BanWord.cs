using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchBot.MVVM.ViewModel;

namespace TwitchBot.MVVM.Model
{
    internal class BanWord
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;

        public BanWord()
        {
            DeleteCommand = new RelayCommand(Delete);
        }

        [NotMapped] public ICommand DeleteCommand { get; set; }

        private void Delete(object obj)
        {
            MessageBox.Show(DataWorker.GetMessageAboutAction( DataWorker.DeleteBanWord(this), 
                "Плохое слово успешно удалено!", 
                "Произошла ошибка. Плохое слово не было удалено!"));

            BanWordsViewModel.StaticBanWords.Remove(this);
        }
    }
}
