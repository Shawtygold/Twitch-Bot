using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.MVVM.Model
{
    class DataWorker
    {
        internal static List<Command> GetCommands()
        {
            List<Command> commands = new();

            try
            {
                using (ApplicationContext db = new())
                {
                    //получение значений из базы данных
                    commands = db.Commands.ToList();
                }
            }
            catch { }

            return commands;
        }
    }
}
