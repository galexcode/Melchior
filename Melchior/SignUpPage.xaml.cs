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
using Melchior.RequestsLibrary;
using Melchior.Request.Common;

namespace Melchior
{
    public partial class SignUpPage : PhoneApplicationPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = new SignUpViewModel();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as SignUpViewModel);
            var signUpCommand = AuthorizationContext.Instance.AuthRequestsLibrary.CreateSignUpCommand(viewModel.PhoneNumber, viewModel.Name, viewModel.SurName, 1, null, false, null, true);
            signUpCommand.ExecuteCompleted += (xsender, xe) =>
            {
                var sid = xe.Result;
                if (xe.Error != null)
                {
                    if (xe.Error is VKRequestCustomException)
                    {
                        var errorCode = ((VKRequestCustomException)xe.Error).Code;
                        if (errorCode == 1112) // Processing.. Try later
                        {
                            signUpCommand.Parameters[signUpCommand.Parameters.Count - 1].Value = sid;
                            signUpCommand.Execute();
                            return;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(xe.Error);
                    return;
                }
                if (xe.Result == null)
                {
                    System.Diagnostics.Debug.WriteLine("Result is empty");
                    return;
                }
                var uri = new Uri(String.Concat("/SignUpConfirmPage.xaml?sid=", sid), UriKind.Relative);
                NavigationService.Navigate(uri);
            };
            signUpCommand.Execute();
        }
    }
}