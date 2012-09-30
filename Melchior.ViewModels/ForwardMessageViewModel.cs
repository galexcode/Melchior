using System;
using System.Collections.Generic;
using Melchior.Models;
using Melchior.RequestsLibrary;
using Melchior.Common;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Melchior.ViewModels.AttachmentsViewModel;
namespace Melchior.ViewModels
{
    public class ForwardMessageViewModel : BaseViewModel
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string Date { get; private set; }

        private string userName;
        public string UserName
        {
            get { return userName; }
            private set
            {
                userName = value;
                NotifyPropertyChanged("UserName");
            }
        }
        private string userPhotoLink;
        public string UserPhotoLink
        {
            get { return userPhotoLink; }
            private set
            {
                userPhotoLink = value;
                NotifyPropertyChanged("UserPhotoLink");
            }
        }
        public ForwardMessageViewModel(string accessToken, MessageInfo message)
        {
            Title = message.Title;
            Body = message.Body;
            Date = message.Date;

            var usersRequestsLibrary = new UsersRequestsLibrary(accessToken);
            var getUsersCommand = usersRequestsLibrary.CreateGetUsersCommand(message.UserId, new string[] { UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink });
            getUsersCommand.ExecuteCompleted += (sender, e) =>
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
                UserName = String.Concat(user.FirstName, Chars.Space, user.LastName);
                UserPhotoLink = user.PhotoLink;
            };
            getUsersCommand.Execute(true);
        }
    }
}
