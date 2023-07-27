using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace TwitchBot.MVVM.Model
{
    class DataWorker
    {
        #region Работа с Command

        internal static ObservableCollection<Command> GetCommands()
        {
            ObservableCollection<Command> commands = new();

            try
            {
                using (ApplicationContext db = new())
                {
                    //получение значений из базы данных
                    commands = new(db.Commands.ToList());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return commands;
        }

        internal static bool AddCommand(Command command)
        {
            bool isAdded = false;

            if(command != null)
            {
                using (ApplicationContext db = new())
                {
                    db.Commands.Add(command);
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

        internal static void ChangeIsEnabledInCommand(int id, bool isEnabled)
        {
            try
            {
                using (ApplicationContext db = new())
                {
                    for (int i = 0; i < db.Commands.ToList().Count; i++)
                    {
                        //поиск по Id в бд
                        if (db.Commands.ToList()[i].Id == id)
                        {
                            //сохранение значения
                            db.Commands.ToList()[i].IsEnabled = isEnabled;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

        #region Работа с Timer

        internal static ObservableCollection<Timer> GetTimers()
        {
            ObservableCollection<Timer> timers = new();

            try
            {
                using (ApplicationContext db = new())
                {
                    //получение значений из базы данных
                    timers = new(db.Timers.ToList());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return timers;
        }

        internal static bool AddTimer(Timer timer)
        {
            bool isAdded = false;

            if (timer != null)
            {
                using (ApplicationContext db = new())
                {
                    db.Timers.Add(timer);
                    db.SaveChanges();

                    isAdded = true;
                }
            }

            return isAdded;
        }

        internal static bool EditTimer(Timer timer)
        {
            bool isEdit = false;

            if (timer != null)
            {
                using (ApplicationContext db = new())
                {
                    for (int i = 0; i < db.Timers.ToList().Count; i++)
                    {
                        if (timer.Id == db.Timers.ToList()[i].Id)
                        {
                            db.Timers.ToList()[i].Title = timer.Title;
                            db.Timers.ToList()[i].Interval = timer.Interval;
                            db.Timers.ToList()[i].ResponceMessage = timer.ResponceMessage;
                            db.Timers.ToList()[i].AutoReset = timer.AutoReset;
                            db.Timers.ToList()[i].MessageInterval = timer.MessageInterval;

                            db.SaveChanges();

                            isEdit = true;
                        }
                    }
                }
            }

            return isEdit;
        }

        internal static bool DeleteTimer(Timer timer)
        {
            bool isDelete = false;

            if (timer != null)
            {
                using (ApplicationContext db = new())
                {
                    db.Timers.Remove(timer);
                    db.SaveChanges();

                    isDelete = true;
                }
            }

            return isDelete;
        }

        internal static void ChangeIsEnabledInTimer(int id, bool isEnabled)
        {
            try
            {
                using (ApplicationContext db = new())
                {
                    for (int i = 0; i < db.Timers.ToList().Count; i++)
                    {
                        //поиск по Id в бд
                        if (db.Timers.ToList()[i].Id == id)
                        {
                            //сохранение значения
                            db.Timers.ToList()[i].IsEnabled = isEnabled;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion


        #region Работа с BanWord

        internal static ObservableCollection<BanWord> GetBanWords()
        {
            ObservableCollection<BanWord> banWords = new();

            try
            {
                using (ApplicationContext db = new())
                {
                    //получаю все плохие слова из базы данных
                    banWords = new (db.BanWords.ToList());
                }
            }
            catch (Exception ex){ MessageBox.Show(ex.Message); }

            return banWords;
        }

        internal static bool AddBanWord(BanWord banWord)
        {
            bool isAdded = false;

            if(banWord != null)
            {
                using (ApplicationContext db = new())
                {
                    db.BanWords.Add(banWord);
                    db.SaveChanges();

                    isAdded = true;
                }
            }

            return isAdded;
        }

        internal static bool DeleteBanWord(BanWord banWord)
        {
            bool isDeleted = false; 

            if(banWord != null)
            {
                using (ApplicationContext db = new())
                {
                    db.BanWords.Remove(banWord);
                    db.SaveChanges();

                    isDeleted = true;
                }
            }

            return isDeleted;
        }
 
        #endregion

        internal static bool InputValidation(params string[] strings)
        {
            for(int i = 0; i < strings.Length; i++)
            {
                //если строка null или пустая
                if (string.IsNullOrEmpty(strings[i]))
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool InputValidation(int[] ints, params string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                //если строка null или пустая
                if (string.IsNullOrEmpty(strings[i]))
                {
                    return  false;
                }
            }

            for(int i = 0; i < ints.Length; i++)
            {
                if (ints[i] <= 0)
                {
                    return false;
                }
            }

            return true;
        }

        //получение сообщения о проделанном действии (добавление, удаление, редактирование)
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
