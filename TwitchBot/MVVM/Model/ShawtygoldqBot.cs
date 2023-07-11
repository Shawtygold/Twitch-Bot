using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;

namespace TwitchBot.MVVM.Model
{
    class ShawtygoldqBot
    {
        
        private TwitchClient client;

        public ShawtygoldqBot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("shawtygoldqbot", "bvdwgq2k2sqte0ctvpzwbjpgj0f5y6");

            client = new TwitchClient();
            client.Initialize(credentials, "shawtygoldq");

            client.OnConnected += Client_OnConnected;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            client.OnUserJoined += Client_OnUserJoined;
            client.OnUserLeft += Client_OnUserLeft;
            client.OnModeratorJoined += Client_OnModeratorJoined;

            //получение списка команд из бд
            commands = DataWorker.GetCommands();
        }

        #region Properties

        public string BotStatus { get; set; } = "Off";
        private List<string> UserNames { get; set; } = new();
        private List<string> BadWords { get; set; } = new() { "гомик", "гомосек", "негр", "негритянка", "негрунчик", "негрилла", "кацап", "москаль", "русня", "хохол", "укроп", "жид", "хач", "даун", "педик", "педераст", "пидорас", "пидор", "пидарас", "гей", "шлюха", "блядота", "мать ебал", "иди нахуй" };
        private Dictionary<string, string> variables { get; set; }
        private List<Command> commands;
        #endregion

        #region Events 

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            try
            {
                client.SendMessage("shawtygoldq", "Присоединился");
            }
            catch { }
        }

        private void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            try
            {
                client.SendMessage(e.Channel, "Приветствую! Не проказничать в чате!");
            }
            catch { }
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            try
            {
                for (int i = 0; i < BadWords.Count; i++)
                {
                    if (e.ChatMessage.Message.ToLower().Contains(BadWords[i]))
                        client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromSeconds(30), "Не ругайся, отдохни минут 30");
                }
            }
            catch { }                  
        }

        //Я решил не просто взять и обработать команды в коде. Я решил добавить возможность создавать команды самому не заходя внутрь проекта,
        //используя при этом переменные, которые в свою очередь будут заменяться на соответсвующие значения.
        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            try
            {
                //словарь с переменными
                variables = new Dictionary<string, string>()
                {
                    ["{random_chatter}"] = UserNames[GetRandomNumber(0, UserNames.Count - 1)],
                    ["{user}"] = e.Command.ChatMessage.Username,
                    ["{channel}"] = e.Command.ChatMessage.Channel,
                    //["{color}"] = ChangeColor(e.Command.ChatMessage.Channel)
                };

                //прохожусь по командам
                for (int i  = 0; i < commands.Count; i++)
                {
                    //если пользователь ввел в чат существующую команду и она активна
                    if (e.Command.CommandText.ToLower() == commands[i].Title.ToLower() && commands[i].IsActive == true)
                    {
                        //для начала надо проверить есть ли в команде переменные
                        //прохожусь по словарю с переменными
                        foreach (var variable in variables)
                        {
                            //если команда содержит какую либо переменную(ключ) в словаре
                            if (commands[i].ResponceType.Contains(variable.Key))
                            {
                                //заменяю в responceType переменную(ключ) на метод(занчение)
                                commands[i].ResponceType = commands[i].ResponceType.Replace(variable.Key, variable.Value);
                            }
                            //если команда содержит переменную random 
                            else if (commands[i].ResponceType.Contains("{random")) //например: (random(5, 10)))
                            {
                                try
                                {
                                    //разделение текста команды на слова
                                    List<string> words = commands[i].ResponceType.Split(" ").ToList();
                                    //здесь будут храниться все переменные random (пользователь может ввести несколько таких переменных в одну команду)
                                    List<string> variables = new();
                                    //индексы
                                    List<int> indexes = new();

                                    int countWords = 0;

                                    for (int w = 0; w < words.Count; w++)
                                    {
                                        if (words[w].Contains("{random"))
                                        {
                                            //подсчет количества таких переменных в строке команды
                                            countWords++;
                                            //добавление переменной в список переменных
                                            variables.Add(words[w]);
                                            //добавление индекса переменной в список индексов
                                            indexes.Add(w);
                                        }
                                    }

                                    for (int v = 0; v < variables.Count; v++)
                                    {
                                        //получаю индексы символов для нахождения чисел, которые переданы в скобках 
                                        int index = variables[v].IndexOf('(');
                                        int index2 = variables[v].IndexOf(',');
                                        int index3 = variables[v].IndexOf(')');

                                        //получение первого числа
                                        int number1 = Convert.ToInt32(variables[v].Substring(index + 1, index2 - index - 1));
                                        //получение второго числа
                                        int number2 = Convert.ToInt32(variables[v].Substring(index2 + 1, index3 - index2 - 1));

                                        //рандомное число от первого числа до второго числа
                                        int rndNumber = GetRandomNumber(Convert.ToInt32(number1), Convert.ToInt32(number2));

                                        //замена переменной на значение
                                        variables[v] = $"{rndNumber}";

                                        //вставка значения по индексу
                                        words[indexes[v]] = variables[v];
                                    }

                                    //объединение
                                    commands[i].ResponceType = string.Join(" ", words);
                                }
                                catch { }                                                             
                            }
                        }

                        //отправка сообщение в чат
                        client.SendMessage(e.Command.ChatMessage.Channel, commands[i].ResponceType);
                    } 
                }


                //switch (e.Command.CommandText.ToLower())
                //{
                //    case "discord":
                //        {
                //            client.SendMessage(e.Command.ChatMessage.Channel, "Discord: https://discord.gg/d7zUqVYXYh");
                //        }
                //        break;

                //    case "donate":
                //        {
                //            client.SendMessage(e.Command.ChatMessage.Channel, "Donate: https://www.donationalerts.com/r/shawtygold");
                //        }
                //        break;

                //    case "youtube":
                //        {
                //            client.SendMessage(e.Command.ChatMessage.Channel, "YouTube: https://www.youtube.com/channel/UCJixaKetI10TJTJ4YX2V_Kg");
                //        }
                //        break;

                //    case "telegram":
                //        {
                //            client.SendMessage(e.Command.ChatMessage.Channel, "Telegram: https://t.me/+nAFnNgTUJq85OTM6");
                //        }
                //        break;

                //    case "ручник":
                //        {
                //            int number = GetRandomIndex(0, 25);
                //            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" размер твоего ручника - {number} см!");
                //        }
                //        break;

                //    case "обнять":
                //        {
                //            int index = GetRandomIndex(0, UserNames.Count - 1);
                //            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" обнял {UserNames[index]}");
                //        }
                //        break;

                //    case "поцелуй":
                //        {
                //            int index = GetRandomIndex(0, UserNames.Count - 1);
                //            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" поцеловал {UserNames[index]}");
                //        }
                //        break;

                //    case "рукопожатие":
                //        {
                //            int index = GetRandomIndex(0, UserNames.Count - 1);
                //            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" пожал руку {UserNames[index]}");
                //        }
                //        break;

                //    case "вертушка":
                //        {
                //            int index = GetRandomIndex(0, UserNames.Count - 1);
                //            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" прописал с вертухи {UserNames[index]}");
                //        }
                //        break;

                //    default:
                //        {
                //            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + ", такой команды не существует!");
                //        }
                //        break;
                //}
            }
            catch { }            
        }

        //почему-то срабатывает с реальными пользователями через некоторое время, а не сразу, возможно это твич хренью страдает
        private void Client_OnUserJoined(object? sender, OnUserJoinedArgs e)
        {
            try
            {
                //добавление имени пользователя при подключении             
                UserNames.Add(e.Username);
            }
            catch { }
        }

        private void Client_OnUserLeft(object? sender, OnUserLeftArgs e)
        {
            try
            {
                //удаление имени пользователя при отключении
                UserNames.Remove(e.Username);
            }
            catch { }         
        }

        private void Client_OnModeratorJoined(object? sender, OnModeratorJoinedArgs e)
        {
            try
            {
                client.SendMessage(e.Channel, $"Модератор {e.Username} на связи!");
            }
            catch { }
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
            catch { }
        }

        //получение рандомного индекса
        private int GetRandomNumber(int from, int to)
        {
            Random rnd = new();
            int number = rnd.Next(from, to);

            return number;
        }       
    }

        //private void ChangeColor(string channel)
        //{
        //    //получение списка цветов
        //    List<ChatColorPresets> colors = Enum.GetValues(typeof(ChatColorPresets)).Cast<ChatColorPresets>().ToList();

        //    //рандомный индекс
        //    int rndIndex = GetRandomNumber(0, colors.Count);

        //    //изменение цвета
        //    client.ChangeChatColor(channel, colors[rndIndex]);
           
        //}

        #endregion
}

