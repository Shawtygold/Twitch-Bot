using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.MVVM.Model
{
    class Timer
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ResponceMessage { get; set; } = null!;
        public int Interval { get; set; }
        public bool IsActive { get; set; }
    }
}
