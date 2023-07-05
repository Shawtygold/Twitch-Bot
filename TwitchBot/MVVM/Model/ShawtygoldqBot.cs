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
        private TwitchClient client;
        //плохих слов гораздо
        private List<string> badWords = new() {"гомик", "гомосек", "негр", "негритянка", "негрунчик", "негрилла", "кацап", "москаль", "русня", "хохол", "укроп", "жид", "хач", "даун", "педик", "педераст", "пидорас", "пидор", "пидарас", "гей", "шлюха", "блядота", "мать ебал", "иди нахуй", "тест" };

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
            client.OnModeratorLeft += Client_OnModeratorLeft;

            client.Connect();
        }

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            client.SendMessage(e.Channel, "Приветствую! Не проказничать в чате!");
        }

        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            for(int i = 0; i < badWords.Count; i++)
            {
                if (e.ChatMessage.Message.ToLower().Contains(badWords[i]))
                    //client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromSeconds(30), "Не ругайся, отдохни 30 минут");
                    client.SendMessage(e.ChatMessage.Channel, "не ругайся");
            }
            
        }

        private void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            
        }

        private void Client_OnUserJoined(object? sender, OnUserJoinedArgs e)
        {
            
        }

        private void Client_OnUserLeft(object? sender, OnUserLeftArgs e)
        {
            
        }

        private void Client_OnModeratorJoined(object? sender, OnModeratorJoinedArgs e)
        {
            
        }

        private void Client_OnModeratorLeft(object? sender, OnModeratorLeftArgs e)
        {
            
        }
    }
}
