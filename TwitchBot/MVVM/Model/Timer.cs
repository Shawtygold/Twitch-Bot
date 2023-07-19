using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Windows.Documents;
using System.Windows.Input;
using TwitchBot.Core;
using TwitchLib.Client;
using System.Collections.ObjectModel;
using System.Threading;

namespace TwitchBot.MVVM.Model
{
    class Timer
    {
        /// <summary>
        /// Как работает таймер:
        /// После того, как таймер был запущен мы ждем когда он сработает. После того, как срабатывает таймер и вызвается событие Timer_Elapsed необходимо проверить 
        /// сколько сообщений было отправлено пользователями до сработки (MessageInterval). Проверка нужна для того, чтобы не было спама сообщениями таймера в чате.
        /// Пока не будет набрано необходимое количество сообщений в чате таймер будет остановлен. Сразу после того как наберется нужное количество сообщений отправится
        /// сообщение таймера в чат. Если AutoRset = true, то цикл идет заново.
        /// </summary>

        public Timer()
        {
            CheckedCommand = new RelayCommand(Checked);
            UncheckedCommand = new RelayCommand(Unchecked);
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ResponceMessage { get; set; } = null!;
        public int Interval { get; set; }
        public bool IsEnabled { get; set; }
        public bool AutoReset { get; set; }
        public int MessageInterval { get; set; }


        //количество сообщений за период от последнего срабатывания таймера
        [NotMapped] internal int countMessssagePeriod = 0;
        [NotMapped] private System.Timers.Timer timer1;
        [NotMapped] private TwitchClient client;
        [NotMapped] private string channel;

        #region Commands

        [NotMapped] public ICommand CheckedCommand { get; set; }
        [NotMapped] public ICommand UncheckedCommand { get; set; }

        private void Checked(object obj)
        {
            //изменяю значение IsActive в базе данных на true
            DataWorker.ChangeIsEnabledInTimer(Id, isEnabled: true);
        }
        private void Unchecked(object obj)
        {
            //изменяю значение IsActive в базе данных на true
            DataWorker.ChangeIsEnabledInTimer(Id, isEnabled: false);
        }

        #endregion

        #region Methods

        internal void Start(TwitchClient client, string channel)
        {
            //тустановка таймера на время равное Interval
            timer1 = new(Interval);
            timer1.Elapsed += Timer_Elapsed;
            timer1.AutoReset = AutoReset;
            timer1.Enabled = IsEnabled;

            this.client = client;
            this.channel = channel;
        }
        internal void Stop()
        {
            if(timer1 != null)
            {
                timer1.Stop();
                timer1.Dispose();
            }
        }

        #endregion

        #region Events

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {            
            //остановка таймера
            timer1.Stop();

            //запуск фонового потока
            Thread thread = new(SendMessage);
            thread.Start();                    
        }

        private void SendMessage()
        {
            //ждем, пока не наберется нужное количество сообщений в чате.
            while (countMessssagePeriod != MessageInterval)
            {
                Thread.Sleep(1000);
            }

            //на всякий случай проверяем
            if(countMessssagePeriod == MessageInterval)
            {
                //отправка сообщения в чат
                client.SendMessage(channel, ResponceMessage);

                //обнуляем countMessssagePeriod
                countMessssagePeriod = 0;
            }            

            if(AutoReset == true)
            {
                timer1.Start();
            }
        }

        #endregion
    }
}
