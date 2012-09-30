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
using Melchior.ViewModels;

namespace Melchior
{
    public partial class RequestsPage : PhoneApplicationPage
    {
        
        

        public RequestsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);
            
            

            var getRequestsCommandsCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateGetRequestsCommand();
            getRequestsCommandsCommand.ExecuteCompleted += (sender, e) =>
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
                RequestsList.ItemsSource = e.Result.Select(x => new RequestViewModel(MelchiorContext.Instance.AccessToken, x)).ToList();
            };
            getRequestsCommandsCommand.Execute(true);

            var getSuggestionsCommandsCommand = MelchiorContext.Instance.FriendsRequestsLibrary.CreateGetSuggestionsCommand();
            getSuggestionsCommandsCommand.ExecuteCompleted += (sender, e) =>
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
                SuggestionsList.ItemsSource = e.Result.Select(x => new ContactViewModel(MelchiorContext.Instance.AccessToken, x.UserId)).ToList();
            };
            getSuggestionsCommandsCommand.Execute(true);
        }

        private void ShowRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var userId = (string)((sender as HyperlinkButton).CommandParameter);
            if (!String.IsNullOrEmpty(userId))
            {
                var uri = new Uri(String.Concat("/RequestPage.xaml?UserId=", userId), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }

        private void ShowProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var userId = (string)(sender as HyperlinkButton).CommandParameter;
            if (!String.IsNullOrEmpty(userId))
            {
                var uri = new Uri(String.Concat("/ProfilePage.xaml?UserId=", userId), UriKind.Relative);
                NavigationService.Navigate(uri);
            }
        }
    }
}