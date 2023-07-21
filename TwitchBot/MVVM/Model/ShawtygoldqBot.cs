using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;

namespace TwitchBot.MVVM.Model
{
    class ShawtygoldqBot
    {
        //Twitch client
        private TwitchClient client;

        #region BotSettings

        private readonly string channel = "shawtygoldq";
        private readonly string oAuth = "bvdwgq2k2sqte0ctvpzwbjpgj0f5y6";
        private readonly string botName = "shawtygoldqbot";

        #endregion

        #region Properties

        internal string BotStatus { get; set; } = "Off";
        private Dictionary<string, string> Variables { get; set; }
        private List<Command> Commands { get; set; }
        private List<Timer> Timers { get; set; } = new();
        private List<string> UserNames { get; set; } = new();     
        private List<string> BadWords { get; set; } = new() { "гомик", "гомосек", "негр", "негритянка", "негрунчик", "негрилла", "кацап", "москаль", "русня", "хохол", "укроп", "жид", "хач", "даун", "педик", "педераст", "пидорас", "пидор", "пидарас", "гей", "шлюха", "блядота", "мать ебал", "иди нахуй" };
        private ObservableCollection<string> Messages { get; set; } = new();

        //имена модераторов или ботов хз как их назвать, которые автоматом подключаются к чату на Twitch
        private List<string> TwitchBotNames { get; set; } = new() { $"shawtygoldqbot", "shawtygoldq", "wannabemygamerfriend", "0ax2", "kattah", "drapsnatt", "aliceydra", "commanderroot", "anotherttvviewer", "01olivia", "01ella", "streamelements", "maria_anderson_", "lurxx" };
        private List<string> HelloNames { get; set; } = new();

        #endregion

        public ShawtygoldqBot()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        #region Events 
        private void Messages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //добавление всем таймерам значения переменной, отвечающей за количество отправленых сообщений за период
            for (int i = 0; i < Timers.Count; i++)
            {
                if (Timers[i].IsEnabled == true)
                {
                    Timers[i].countMessssagePeriod++;
                }
            }
        }

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            try
            {
                //получение списка команд из бд
                Commands = new(DataWorker.GetCommands());

                //получение списка таймеров из бд
                Timers = new(DataWorker.GetTimers());

                //запуск всех таймеров
                for (int i = 0; i < Timers.Count; i++)
                {
                    //если таймеры включены+
                    if (Timers[i].IsEnabled == true)
                    {
                        Timers[i].Start(client, channel);
                    }
                }
            }
            catch { }
        }

        private void Client_OnDisconnected(object? sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            try
            {
                //останавливаю все таймеры
                for (int i = 0; i < Timers.Count; i++)
                {
                    Timers[i].Stop();
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            try
            {
                client.SendMessage(e.Channel, "Здарова стример!");             
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            try
            {
                //если не начинается с "!"
                if (e.ChatMessage.Message.StartsWith("!") == false)
                {
                    //добавляю сообщение в список сообщений
                    Messages.Add(e.ChatMessage.Message);

                    //проверка на плохие слова
                    for (int i = 0; i < BadWords.Count; i++)
                    {
                        //в таймаут плохих челов
                        if (e.ChatMessage.Message.ToLower().Contains(BadWords[i]))
                            client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(15), "Не ругайся, отдохни минут 15");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //Я решил не просто взять и обработать команды в коде. Я решил добавить возможность создавать команды самому не заходя внутрь проекта,
        //используя при этом переменные, которые в свою очередь будут заменяться на соответсвующие значения.
        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            try
            {
                //добавляю сообщение в список сообщений
                Messages.Add(e.Command.ChatMessage.Message);

                //словарь с переменными
                Variables = new Dictionary<string, string>()
                {
                    ["{random_chatter}"] = UserNames[GetRandomNumber(0, UserNames.Count - 1)],
                    ["{user}"] = e.Command.ChatMessage.Username,
                    ["{channel}"] = e.Command.ChatMessage.Channel
                };

                for (int i  = 0; i < Commands.Count; i++)
                {
                    if (e.Command.CommandText.ToLower() == Commands[i].Title.ToLower() && Commands[i].IsEnabled == true)
                    {
                        //использую эту переменную для того чтобы вместо оригинального текста команды заменять переменные здесь. (чтобы оригинальнй reponceType не был изменен)
                        string responceType = Commands[i].ResponceType;

                        //для начала надо проверить есть ли в команде переменные
                        //прохожусь по словарю с переменными
                        foreach (var variable in Variables)
                        {
                            //если команда содержит какую либо переменную(ключ) в словаре
                            if (responceType.Contains(variable.Key))
                            {
                                //заменяю в responceType переменную(ключ) на метод(значение)
                                responceType = responceType.Replace(variable.Key, variable.Value);
                            }
                            //если команда содержит переменную random 
                            else if (responceType.Contains("{random")) //например: {random(5, 10)}
                            {
                                //разделение текста команды на слова
                                List<string> words = responceType.Split(" ").ToList();
                                //здесь будут храниться все переменные random (пользователь может ввести несколько таких переменных в одну команду)
                                List<string> variables = new();
                                //индексы
                                List<int> indexes = new();

                                for (int j = 0; j < words.Count; j++)
                                {
                                    if (words[j].Contains("{random"))
                                    {
                                        //добавление переменной в список переменных
                                        variables.Add(words[j]);
                                        //добавление индекса переменной в список индексов
                                        indexes.Add(j);
                                    }
                                }

                                for (int k = 0; k < variables.Count; k++)
                                {
                                    //получаю индексы символов для нахождения чисел, которые переданы в скобках 
                                    int index = variables[k].IndexOf('(');
                                    int index2 = variables[k].IndexOf(',');
                                    int index3 = variables[k].IndexOf(')');

                                    //получение первого числа
                                    int number1 = Convert.ToInt32(variables[k].Substring(index + 1, index2 - index - 1));
                                    //получение второго числа
                                    int number2 = Convert.ToInt32(variables[k].Substring(index2 + 1, index3 - index2 - 1));

                                    //рандомное число от первого числа до второго числа
                                    int rndNumber = GetRandomNumber(Convert.ToInt32(number1), Convert.ToInt32(number2));

                                    //замена переменной на значение
                                    variables[k] = $"{rndNumber}";

                                    //вставка значения по индексу
                                    words[indexes[k]] = variables[k];
                                }

                                //объединение
                                responceType = string.Join(" ", words);                                                                                           
                            }
                        }

                        //отправка сообщение в чат
                        client.SendMessage(e.Command.ChatMessage.Channel, responceType);
                    } 
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnUserJoined(object? sender, OnUserJoinedArgs e)
        {
            try
            {
                //добавление имени пользователя при подключении
                UserNames.Add(e.Username);

                if (BotCheck(e.Username) == false)
                {
                    if (HelloCheck(e.Username) == false)
                    {
                        client.SendMessage(e.Channel, $"Привет, {e.Username}!");
                        HelloNames.Add(e.Username);
                    }
                }                  
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //private void Client_OnUserLeft(object? sender, OnUserLeftArgs e)
        //{
        //    try
        //    {
        //        //удаление имени пользователя при отключении
        //        UserNames.Remove(e.Username);
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        private void Client_OnModeratorJoined(object? sender, OnModeratorJoinedArgs e)
        {
            try
            {
                client.SendMessage(e.Channel, $"Модератор {e.Username} на связи!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

        #region Methods

        internal void Connect()
        {
            try
            {
                ConnectionCredentials credentials = new(botName, oAuth);

                client = new TwitchClient();
                client.Initialize(credentials, channel);

                client.OnConnected += Client_OnConnected;
                client.OnJoinedChannel += Client_OnJoinedChannel;
                client.OnMessageReceived += Client_OnMessageReceived;
                client.OnChatCommandReceived += Client_OnChatCommandReceived;
                client.OnUserJoined += Client_OnUserJoined;
                //client.OnUserLeft += Client_OnUserLeft;
                client.OnModeratorJoined += Client_OnModeratorJoined;
                client.OnDisconnected += Client_OnDisconnected;
                client.OnNewSubscriber += Client_OnNewSubscriber;

                //подключаем бота
                client.Connect();

                BotStatus = "On";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnNewSubscriber(object? sender, OnNewSubscriberArgs e)
        {
            try
            {
                //отправка сообщения при новом подписчике
                client.SendMessage(e.Channel, "Спасибо за подписку, " + e.Subscriber.DisplayName);
            }
            catch { }
        }

        internal void Disconnect()
        {
            try
            {
                //отключаем бота
                client.Disconnect();

                BotStatus = "Off";
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private static int GetRandomNumber(int from, int to)
        {
            Random rnd = new();
            int number = rnd.Next(from, to);

            return number;
        }

        private bool BotCheck(string uername)
        {
            bool bot = false;

            for(int i  = 0; i < TwitchBotNames.Count; i++)
            {
                //если имя совпадает с именем бота
                if(uername == TwitchBotNames[i])
                {
                    bot = true; 
                }
            }

            return bot;
        }

        //метод проверяющий приветствовал ли бот пользователя в чате
        private bool HelloCheck(string username)
        {
            bool result = false;

            for(int i = 0; i < HelloNames.Count; i++)
            {
                //если пользователя приветствовали
                if(username == HelloNames[i])
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion
    }
}

