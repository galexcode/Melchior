using System;
using Melchior.Models;
using System.Windows.Media.Imaging;
using Melchior.Common;
using Melchior.RequestsLibrary;

namespace Melchior.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        public string UserId { get; set; }

        private string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
            private set
            {
                firstName = value;
                NotifyPropertyChanged("FirstName");

            }
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
                NotifyPropertyChanged("Name");

            }
        }

        private string userPhotoLink;
        public string UserPhotoLink
        {
            get
            {
                return userPhotoLink;
            }
            private set
            {
                userPhotoLink = value;
                NotifyPropertyChanged("UserPhotoLink");
            }
        }

        private bool userIsOnline;
        public bool UserIsOnline
        {
            get
            {
                return userIsOnline;
            }
            set
            {
                userIsOnline = value;
                NotifyPropertyChanged("UserIsOnline");
            }
        }

        public ContactViewModel(string accessToken, string userId)
        {
            UserId = userId;

            var messages = new UsersRequestsLibrary(accessToken);
            var getUsersCommand = messages.CreateGetUsersCommand(userId, new string[] { UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink });
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
                FirstName = user.FirstName;
                Name = String.Concat(FirstName, Chars.Space, user.LastName);
                UserPhotoLink = user.PhotoLink;
                UserIsOnline = user.IsOnline;
            };
            getUsersCommand.Execute(true);
        }
        public ContactViewModel(UserInfo user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            Name = String.Concat(FirstName, Chars.Space, user.LastName);
            UserPhotoLink = user.PhotoLink;
            UserIsOnline = user.IsOnline;
        }
    }
}