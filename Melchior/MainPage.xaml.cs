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
using Melchior.RequestsLibrary;
using Melchior.Models;

namespace Melchior
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LoginViewModel() { Login = "89853031997", Password = "1pyfrpjlbfrf" };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as LoginViewModel);
            if (String.IsNullOrEmpty(viewModel.Login) || String.IsNullOrEmpty(viewModel.Password))
            {
                return;
            }

            var authCommand = new AutorizationRequestCommand(viewModel.Login, viewModel.Password);
            authCommand.ExecuteCompleted += (xsender, xe) =>
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
                MelchiorContext.Instance.Authorize(xe.Result.AccessToken, xe.Result.UserId);
                var uri = new Uri(String.Concat("/PivotPage.xaml"), UriKind.Relative);
                NavigationService.Navigate(uri);
            };
            authCommand.Execute();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("/SignUpPage.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}