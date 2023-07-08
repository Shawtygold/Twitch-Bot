using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.MVVM.Model
{
    internal class Command
    {



        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ResponceType { get; set; } = null!; //то, что выводит бот при вводе команды
        public bool IsActive { get; set; } = false;   
    }
}
