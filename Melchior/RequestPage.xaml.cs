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

namespace Melchior
{
    public partial class RequestPage : PhoneApplicationPage
    {
        public string UserId { get; set; }

        public RequestPage()
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

        private void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var addFriendCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateAddFriendCommand(UserId);
            addFriendCommand.ExecuteCompleted += (xsender, xe) =>
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
                
                NavigationService.RemoveBackEntry();

                var uri = new Uri(String.Concat("/ProfilePage.xaml?UserId=", UserId), UriKind.Relative);
                NavigationService.Navigate(uri);  
            };
            addFriendCommand.Execute(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteRequestCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateDeleteFriendCommand(UserId);
            deleteRequestCommand.ExecuteCompleted += (xsender, xe) =>
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
                NavigationService.GoBack();
            };
            deleteRequestCommand.Execute(true);
        }
    }
}