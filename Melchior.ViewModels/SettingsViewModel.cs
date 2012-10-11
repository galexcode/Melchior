using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Melchior.Models;
using Melchior.Common;
using System.Windows.Media.Imaging;

namespace Melchior.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public string UserName { get; set; }
        private BitmapImage userPhoto;
        public BitmapImage UserPhoto
        {
            get
            {
                return userPhoto;
            }
            set
            {
                userPhoto = value;
                NotifyPropertyChanged("UserPhoto");
            }
        }

        public const string SwitchVibratePropertyName = "SwitchVibrate";
        private int switchVibrate;
        public int SwitchVibrate
        {
            get
            {
                return switchVibrate;
            }
            set
            {
                switchVibrate = value;
                NotifyPropertyChanged(SwitchVibratePropertyName);
            }
        }

        public const string SwitchSoundPropertyName = "SwitchSound";
        private int switchSound;
        public int SwitchSound
        {
            get
            {
                return switchSound;
            }
            set
            {
                switchSound = value;
                NotifyPropertyChanged(SwitchSoundPropertyName);
            }
        }

        public const string SwitchHintPropertyName = "SwitchHint";
        private int switchHint;
        public int SwitchHint
        {
            get
            {
                return switchHint;
            }
            set
            {
                switchHint = value;
                NotifyPropertyChanged(SwitchHintPropertyName);
            }
        }

        public DateTime DisableNotificationDate { get; set; }

        public SettingsViewModel(UserInfo user)
        {
            UserName = String.Concat(user.FirstName, Chars.Space, user.LastName);
            UserPhoto = new BitmapImage(new Uri(user.PhotoLink));
        }
    }
}