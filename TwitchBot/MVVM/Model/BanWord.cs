using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.MVVM.Model
{
    internal class BanWord
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
    }
}
