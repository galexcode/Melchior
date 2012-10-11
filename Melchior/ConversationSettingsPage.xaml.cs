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
    public partial class ConversationSettingsPage : PhoneApplicationPage
    {
        public string ConversationChatId { get; set; }

        public ConversationSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ConversationChatId = NavigationContext.QueryString.ContainsKey("ConversationChatId") ? NavigationContext.QueryString["ConversationChatId"] : null;

            var getChatUsersCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateGetChatUsersCommand(ConversationChatId, new string[] { UserInfo.FieldUserId, UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink, UserInfo.FieldIsOnline });
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
                ChatUsersList.ItemsSource = xe.Result.Select(x => new ContactViewModel(MelchiorContext.Instance.AccessToken, x.UserId)).ToList();
            };
            getChatUsersCommand.Execute(true);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var uri = new Uri(String.Concat("/FriendsSearchPage.xaml?", "&ConversationChatId=", ConversationChatId), UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var title = TitleTextBox.Text;
            var editChatCommand = MelchiorContext.Instance.MessagesRequestsLibrary.CreateEditChatCommand(ConversationChatId, title);
            editChatCommand.ExecuteCompleted += (xsender, xe) =>
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
            };
            editChatCommand.Execute();

            NavigationService.GoBack();
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            var uri = new Uri(String.Concat("/ConversationSettingsPage.xaml?", "&ConversationChatId=", ConversationChatId), UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}