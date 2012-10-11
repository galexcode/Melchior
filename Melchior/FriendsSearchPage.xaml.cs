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
using Melchior.Models;
using Melchior.ViewModels;

namespace Melchior
{
    public partial class FriendsSearchPage : PhoneApplicationPage
    {
        public string ConversationChatId { get; set; }

        public FriendsSearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConversationChatId = NavigationContext.QueryString.ContainsKey("ConversationChatId") ? NavigationContext.QueryString["ConversationChatId"] : null;

            var getFriendsCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateGetFriendsCommand(MelchiorContext.Instance.OwnerUserId, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink, UserInfo.FieldIsOnline });
            getFriendsCommand.ExecuteCompleted += (sender, xe) =>
            {
                if (xe.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    return;
                }
                if (xe.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                FriendsList.ItemsSource = xe.Result.Select(x => new ContactViewModel(x)).ToList();
            };
            getFriendsCommand.Execute(true);
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var userId = (string)((sender as HyperlinkButton).CommandParameter);

            var addChatUserCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateAddChatUserCommand(ConversationChatId, userId);
            addChatUserCommand.ExecuteCompleted += (xsender, xe) =>
            {
                if (xe.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    return;
                }
                if (!xe.Result)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                RefreshUsersList();
            };
            addChatUserCommand.Execute(true);
        }
        public void RefreshUsersList()
        {
            var getChatUsersCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateGetChatUsersCommand(ConversationChatId, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink });
            getChatUsersCommand.ExecuteCompleted += (xsender, xe) =>
            {
                if (xe.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    return;
                }
                if (xe.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                NavigationService.GoBack();
            };
            getChatUsersCommand.Execute(true, true);
        }
    }
}