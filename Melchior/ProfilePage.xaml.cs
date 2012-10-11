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
using Melchior.RequestsLibrary;
using Melchior.Models;
using Melchior.ViewModels;

namespace Melchior
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        public string UserId { get; set; }

        public ProfilePage()
        {
            InitializeComponent();

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);

            UserId = NavigationContext.QueryString["UserId"];

            var getUserCommand = MelchiorContext.Instance.UsersRequestsLibrary.CreateGetUsersCommand(MelchiorContext.Instance.OwnerUserId, new string[] { UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoBigLink });
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
                if (e.Result.Count == 0) return;

                var user = e.Result[0];
                LayoutRoot.DataContext = new ProfileViewModel(e.Result[0]);
            };
            getUserCommand.Execute(true);
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(String.Concat("/ConversationPage.xaml?UserId=", MelchiorContext.Instance.OwnerUserId, "&ConversationUserId=", UserId), UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveFromFrinds_Click(object sender, EventArgs e)
        {
            //var getMessagesCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateDeleteFriendCommand(Conversation
            //getMessagesCommand.ExecuteCompleted += (sender, e) =>
            //{
            //    if (e.Error != null)
            //    {
            //        System.Diagnostics.Debug.WriteLine(e.Error);
            //        return;
            //    }
            //    if (e.Result == null)
            //    {
            //        System.Diagnostics.Debug.WriteLine("Result is empty");
            //        return;
            //    }
            //    if (e.Result.Count == 0) return;

            //    var user = e.Result[0];
            //    LayoutRoot.DataContext = new ProfileViewModel(e.Result[0]);
            //};
            //getMessagesCommand.Execute(true);
        }
    }
}