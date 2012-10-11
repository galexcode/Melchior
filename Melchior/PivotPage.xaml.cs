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
using System.Collections.ObjectModel;
using Melchior.ViewModels;
using Microsoft.Phone.UserData;
using Melchior.Request.Xml;
using Microsoft.Phone.Shell;

namespace Melchior
{
    public partial class PivotPage : PhoneApplicationPage
    {

        private VKRequestCommand<MessageCollection> GetDialogsCommand { get; set; }
        private VKRequestCommand<MessageCollection> GetMessagesCommand { get; set; }
        private VKRequestCommand<UserCollection> GetFriendsCommand { get; set; }

        public PivotPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);


            GetDialogsCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateGetDialogsCommand();
            GetDialogsCommand.ExecuteCompleted += (sender, e) =>
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
                ConversationsList.ItemsSource = e.Result.Select(message => new ConversationMessageViewModel(MelchiorContext.Instance.AccessToken, message)).ToList();
            };

            GetMessagesCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateGetMessagesCommand();
            GetMessagesCommand.ExecuteCompleted += (sender, e) =>
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
                MessagesList.ItemsSource = e.Result.Select(message => new ConversationMessageViewModel(MelchiorContext.Instance.AccessToken, message)).ToList();
            };

            GetFriendsCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateGetFriendsCommand(MelchiorContext.Instance.OwnerUserId, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink, UserInfo.FieldIsOnline });
            GetFriendsCommand.ExecuteCompleted += (sender, e) =>
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
                var usersStatus = MelchiorContext.Instance.UsersStatus;
                FriendsList.ItemsSource = e.Result.Select(x => new ContactViewModel(x)
                    {
                        UserIsOnline = usersStatus.ContainsKey(x.UserId) ? (usersStatus[x.UserId] == UserStatuses.Online) : x.IsOnline
                    }).ToList();
            };

            GetDialogsCommand.Execute(true);
            GetMessagesCommand.Execute(true);
            GetFriendsCommand.Execute(true);
        }
        public static Microsoft.Phone.UserData.Contact con;

        private void SyncContactsButton_Click(object sender, RoutedEventArgs e)
        {
            SyncContacts.Visibility = Visibility.Collapsed;
            ContactsList.Visibility = Visibility.Visible;

            var contacts = new Contacts();
            contacts.SearchCompleted += (zsender, ze) =>
            {
                var phones = new List<string>();
                foreach (var phoneNumbers in ze.Results.Select(x => x.PhoneNumbers))
                {
                    phones.AddRange(phoneNumbers.Select(x => x.PhoneNumber));
                }
                var getContactsCommandsCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateGetFriendsByPhonesCommand(phones, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink, UserInfo.FieldIsOnline });
                getContactsCommandsCommand.ExecuteCompleted += (xsender, xe) =>
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
                    ContactsList.ItemsSource = xe.Result.Select(x => new ContactViewModel(x)).ToList();
                };
                getContactsCommandsCommand.Execute(true);
            };
            contacts.SearchAsync(null, FilterKind.None, null);

        }

        private void NewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 1;
        }

        private void RequestsButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(String.Concat("/RequestsPage.xaml"), UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void ShowChatConversationLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var conversationChatId = (string)((sender as HyperlinkButton).CommandParameter);
            if (!String.IsNullOrEmpty(conversationChatId))
            {
                var uri = new Uri(String.Concat("/ConversationPage.xaml?", "&ConversationChatId=", conversationChatId), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }
        private void ShowUserConversationLinkButton_Click(object sender, RoutedEventArgs e)
        {
            var conversationUserId = (string)((sender as HyperlinkButton).CommandParameter);
            if (!String.IsNullOrEmpty(conversationUserId))
            {
                var uri = new Uri(String.Concat("/ConversationPage.xaml?", "&ConversationUserId=", conversationUserId), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }

        private void SearchContactsButton_Click(object sender, EventArgs e)
        {

        }

        private void NewMessageButton_Click(object sender, EventArgs e)
        {

        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            GetMessagesCommand.Execute(true, true);
        }

        private void SearchMessageButton_Click(object sender, EventArgs e)
        {

        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            var uri = new Uri("/SettingsPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item == ContactsPivotItem)
            {
            }
        }
    }
}