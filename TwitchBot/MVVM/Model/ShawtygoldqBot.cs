using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<string> userNames = new();
        private TwitchClient client;
        private readonly List<string> badWords = new() {"гомик", "гомосек", "негр", "негритянка", "негрунчик", "негрилла", "кацап", "москаль", "русня", "хохол", "укроп", "жид", "хач", "даун", "педик", "педераст", "пидорас", "пидор", "пидарас", "гей", "шлюха", "блядота", "мать ебал", "иди нахуй" };

        public ShawtygoldqBot()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("shawtygoldqbot", "bvdwgq2k2sqte0ctvpzwbjpgj0f5y6");

            client = new TwitchClient();
            client.Initialize(credentials, "shawtygoldq");

            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            client.OnUserJoined += Client_OnUserJoined;
            client.OnUserLeft += Client_OnUserLeft;
            client.OnModeratorJoined += Client_OnModeratorJoined;
        }


        #region Properties

        public string BotStatus { get; set; } = "Off";

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

        private void Client_OnDisconnected(object? sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            try
            {
                client.SendMessage("shawtygoldq", "Отсоединился");
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
                for (int i = 0; i < badWords.Count; i++)
                {
                    if (e.ChatMessage.Message.ToLower().Contains(badWords[i]))
                        client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromSeconds(30), "Не ругайся, отдохни минут 30");
                }
            }
            catch { }                  
        }

        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            try
            {
                switch (e.Command.CommandText.ToLower())
                {
                    case "discord":
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "Discord: https://discord.gg/d7zUqVYXYh");
                        }
                        break;

                    case "donate":
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "Donate: https://www.donationalerts.com/r/shawtygold");
                        }
                        break;

                    case "youtube":
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "YouTube: https://www.youtube.com/channel/UCJixaKetI10TJTJ4YX2V_Kg");
                        }
                        break;

                    case "telegram":
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, "Telegram: https://t.me/+nAFnNgTUJq85OTM6");
                        }
                        break;

                    case "ручник":
                        {
                            int number = GetRandomIndex(0, 25);
                            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" размер твоего ручника - {number} см!");
                        }
                        break;

                    case "обнять":
                        {
                            int index = GetRandomIndex(0, userNames.Count - 1);
                            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" обнял {userNames[index]}");
                        }
                        break;

                    case "поцелуй":
                        {
                            int index = GetRandomIndex(0, userNames.Count - 1);
                            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" поцеловал {userNames[index]}");
                        }
                        break;

                    case "рукопожатие":
                        {
                            int index = GetRandomIndex(0, userNames.Count - 1);
                            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" пожал руку {userNames[index]}");
                        }
                        break;

                    case "вертушка":
                        {
                            int index = GetRandomIndex(0, userNames.Count - 1);
                            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + $" прописал с вертухи {userNames[index]}");
                        }
                        break;

                    default:
                        {
                            client.SendMessage(e.Command.ChatMessage.Channel, e.Command.ChatMessage.Username + ", такой команды не существует!");
                        }
                        break;
                }
            }
            catch { }            
        }

        //почему-то срабатывает с реальными пользователями через некоторое время, а не сразу, возможно это твич хренью страдает
        private void Client_OnUserJoined(object? sender, OnUserJoinedArgs e)
        {
            try
            {
                //добавление имени пользователя при подключении             
                userNames.Add(e.Username);
            }
            catch { }
        }

        private void Client_OnUserLeft(object? sender, OnUserLeftArgs e)
        {
            try
            {
                //удаление имени пользователя при отключении
                userNames.Remove(e.Username);
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
        private int GetRandomIndex(int from, int to)
        {
            Random rnd = new();
            int index = rnd.Next(from, to);

            return index;
        }

        #endregion
    }
}
