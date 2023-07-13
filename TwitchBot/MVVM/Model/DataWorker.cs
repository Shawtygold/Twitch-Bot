using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        internal static string InputValidation(string title, string responceType)
        {
            string message = "";

            message += Validate(title);
            message += Validate(responceType);

            //если не было ошибок в заполнении
            if(message.Length == 0)
            {
                message = "Ok";
            }

            return message;
        }

        private static string Validate(string property)
        {
            string message = "";

            //если поле пустое
            if (property.Trim() == "")
            {
                message += $"Введите {property}/\n";
            }

            return message;
        }       

        internal static bool AddCommand(Command command)
        {
            bool isAdded = false;

            if(command != null)
            {
                using (ApplicationContext db = new())
                {
                    db.Add(command);
                    db.SaveChanges();

                    isAdded = true;
                }
            }

            return isAdded;
        }

        internal static bool EditCommand(Command command)
        {
            bool isEdit = false;

            if(command != null)
            {
                using(ApplicationContext db = new())
                {
                    for(int i = 0; i < db.Commands.ToList().Count; i++)
                    {
                        if (command.Id == db.Commands.ToList()[i].Id)
                        {
                            db.Commands.ToList()[i].Title = command.Title;
                            db.Commands.ToList()[i].ResponceType = command.ResponceType;

                            db.SaveChanges();

                            isEdit = true;
                        }
                    }
                }
            }

            return isEdit;
        }

        internal static bool DeleteCommand(Command command)
        {
            bool isDelete = false;

            if(command != null)
            {
                using (ApplicationContext db = new())
                {                   
                    db.Commands.Remove(command);
                    db.SaveChanges();

                    isDelete = true;
                }
            }

            return isDelete;
        }

        internal static string GetMessageAboutAction(bool wasTheAction, string msgTrue, string msgFalse)
        {
            string message;

            //если действие было совершено успешно
            if (wasTheAction == true)
            {
                message = msgTrue;
            }
            else
            {
                message = msgFalse;
            }

            return message;
        }
    }
}
