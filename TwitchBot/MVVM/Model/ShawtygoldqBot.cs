using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        //таймер
        private static System.Timers.Timer timer;

        //количество сообщений за период от последнего срабатывания таймера
        private int countMessagePeriod = 0;

        #region BotSettings

        private readonly string channel = "shawtygoldq";
        private readonly string oAuth = "bvdwgq2k2sqte0ctvpzwbjpgj0f5y6";
        private readonly string botName = "shawtygoldqbot";

        #endregion

        #region Properties

        internal string BotStatus { get; set; } = "Off";
        private Dictionary<string, string> Variables { get; set; }
        private List<Command> Commands { get; set; }
        private List<string> UserNames { get; set; } = new();
        private List<string> BadWords { get; set; } = new() { "гомик", "гомосек", "негр", "негритянка", "негрунчик", "негрилла", "кацап", "москаль", "русня", "хохол", "укроп", "жид", "хач", "даун", "педик", "педераст", "пидорас", "пидор", "пидарас", "гей", "шлюха", "блядота", "мать ебал", "иди нахуй" };
        private List<string> Messages { get; set; } = new();

        #endregion

        public ShawtygoldqBot()
        {
            try
            {
                ConnectionCredentials credentials = new (botName, oAuth);

                client = new TwitchClient();
                client.Initialize(credentials, channel);

                client.OnConnected += Client_OnConnected;
                client.OnJoinedChannel += Client_OnJoinedChannel;
                client.OnMessageReceived += Client_OnMessageReceived;
                client.OnChatCommandReceived += Client_OnChatCommandReceived;
                client.OnUserJoined += Client_OnUserJoined;
                client.OnUserLeft += Client_OnUserLeft;
                client.OnModeratorJoined += Client_OnModeratorJoined;
                client.OnDisconnected += Client_OnDisconnected;

                //получение списка команд из бд
                Commands = DataWorker.GetCommands();

                //установка таймера
                SetTimer();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }          
        }

        #region Events 

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            try
            {
                client.SendMessage(channel, "Присоединился");                
            }
            catch { }
        }

        private void Client_OnDisconnected(object? sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            try
            {
                timer.Stop();
                timer.Dispose();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            try
            {
                client.SendMessage(e.Channel, "Приветствую! Не проказничать в чате!");
                //запуск таймера
                timer.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            try
            {
                //добавляю сообщение в список сообщений
                Messages.Add(e.ChatMessage.Message);

                //проверка на плохие слова
                for (int i = 0; i < BadWords.Count; i++)
                {
                    //в таймаут плохих челов
                    if (e.ChatMessage.Message.ToLower().Contains(BadWords[i]))
                        client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromSeconds(30), "Не ругайся, отдохни минут 30");
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

                //прохожусь по командам
                for (int i  = 0; i < Commands.Count; i++)
                {
                    //если пользователь ввел в чат существующую команду и она активна
                    if (e.Command.CommandText.ToLower() == Commands[i].Title.ToLower() && Commands[i].IsActive == true)
                    {
                        //для начала надо проверить есть ли в команде переменные
                        //прохожусь по словарю с переменными
                        foreach (var variable in Variables)
                        {
                            //если команда содержит какую либо переменную(ключ) в словаре
                            if (Commands[i].ResponceType.Contains(variable.Key))
                            {
                                //заменяю в responceType переменную(ключ) на метод(значение)
                                Commands[i].ResponceType = Commands[i].ResponceType.Replace(variable.Key, variable.Value);
                            }
                            //если команда содержит переменную random 
                            else if (Commands[i].ResponceType.Contains("{random")) //например: (random(5, 10)))
                            {
                                //разделение текста команды на слова
                                List<string> words = Commands[i].ResponceType.Split(" ").ToList();
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
                                Commands[i].ResponceType = string.Join(" ", words);                                                                                           
                            }
                        }

                        //отправка сообщение в чат
                        client.SendMessage(e.Command.ChatMessage.Channel, Commands[i].ResponceType);
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
                //приветствие
                client.SendMessage(e.Channel, $"Привет, {e.Username}!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnUserLeft(object? sender, OnUserLeftArgs e)
        {
            try
            {
                //удаление имени пользователя при отключении
                UserNames.Remove(e.Username);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Client_OnModeratorJoined(object? sender, OnModeratorJoinedArgs e)
        {
            try
            {
                client.SendMessage(e.Channel, $"Модератор {e.Username} на связи!");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            //если бот подключен к каналу
            if (client.JoinedChannels.Count != 0)
            {
                client.SendMessage(channel, "Нажимая на кнопку \"Отслеживать\" Вы даёте +1000 к мотивации стримера, а также Вы будете в курсе о всех последующих стримах! Приятного просмотра!");
            }
        }

        #endregion

        #region Methods

        internal void Connect()
        {
            try
            {                                
                //подключаем бота
                client.Connect();

                BotStatus = "On";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        //получение рандомного индекса
        private static int GetRandomNumber(int from, int to)
        {
            Random rnd = new();
            int number = rnd.Next(from, to);

            return number;
        }

        private void SetTimer()
        {
            //таймер на 15 мин
            timer = new System.Timers.Timer(900000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        #endregion
    }
}

