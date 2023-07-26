using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.MVVM.Model;
using TwitchBot.Services;
using Wpf.Ui.Common.Interfaces;

namespace TwitchBot.MVVM.ViewModel
{
    internal class BanWordsViewModel : Core.ViewModel
    {
        public BanWordsViewModel(INavigationService navigation)
        {
            Naviagtion = navigation;
        }

        #region Properties


        private INavigationService _navigation;
        public INavigationService Naviagtion
        {
            get { return _navigation; }
            set { _navigation = value; OnPropertyChanged(); }
        }


        private List<BanWord> _banWords = new();
        public List<BanWord> BanWords
        {
            get { return _banWords; }
            set { _banWords = value; OnPropertyChanged(); }
        }


        private static List<BanWord> _staticBanWords = new();
        public static List<BanWord> StaticBanWords
        {
            get { return _staticBanWords; }
            set { _staticBanWords = value; }
        }



        #endregion
    }
}
