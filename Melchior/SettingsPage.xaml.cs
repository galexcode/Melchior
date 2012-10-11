using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Melchior.ViewModels;
using Melchior.Models;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace Melchior
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);

            var getUserCommand = MelchiorContext.Instance.UsersRequestsLibrary.CreateGetUsersCommand(MelchiorContext.Instance.OwnerUserId, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink });
            getUserCommand.ExecuteCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(e.Error);
                    return;
                }
                if (e.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                if (e.Result.Count == 1)
                {
                    var user = e.Result[0];
                    var viewModel = new SettingsViewModel(user);
                    viewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(viewModel_PropertyChanged);
                    DataContext = viewModel;
                }
            };
        }

        public void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var viewModel = (SettingsViewModel)sender;
            switch (e.PropertyName)
            {
                case SettingsViewModel.SwitchVibratePropertyName: MelchiorContext.Instance.SwitchVibrate = viewModel.SwitchVibrate == 1; break;
                case SettingsViewModel.SwitchSoundPropertyName: MelchiorContext.Instance.SwitchSound = viewModel.SwitchSound == 1; break;
                case SettingsViewModel.SwitchHintPropertyName: MelchiorContext.Instance.SwitchHint = viewModel.SwitchHint == 1; break;
                default: break;
            }
        }

        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new PhotoChooserTask();
            task.Completed += (xsender, xe) =>
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(xe.ChosenPhoto);
                (DataContext as SettingsViewModel).UserPhoto = bitmap;
            };
            task.Show();
        }

        private void DisableNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DateTime.Now.AddHours(8.0);
            MelchiorContext.Instance.DisableNotificationDate = date;
            DisableNotificationInfoTextBlock.Text = String.Format(StringResources.Disable_notification, date.ToString("dd.MM.yyyy"));
        }

        private void DisableNotificationOnHourButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DateTime.Now.AddHours(1.0);
            MelchiorContext.Instance.DisableNotificationDate = date;
            DisableNotificationInfoTextBlock.Text = String.Format(StringResources.Disable_notification, date.ToString("dd.MM.yyyy"));
        }

        private void LogOffButton_Click(object sender, RoutedEventArgs e)
        {
            MelchiorContext.Instance.LogOff();
            var uri = new Uri("/MainPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MelchiorContext.Instance.DisableNotificationDate = null;
        }
    }
}