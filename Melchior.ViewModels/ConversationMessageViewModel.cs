using System;
using Melchior.Models;
using Melchior.RequestsLibrary;
using Melchior.Common;
using System.Windows.Media.Imaging;
namespace Melchior.ViewModels
{
    public class ConversationMessageViewModel : BaseViewModel
    {
        public string ChatId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string Date { get; private set; }

        private string userId;
        public string UserId
        {
            get { return userId; }
            private set
            {
                userId = value;
                NotifyPropertyChanged("UserId");
            }
        }
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
        private BitmapImage userPhoto;
        public BitmapImage UserPhoto
        {
            get
            {
                return userPhoto;
            }
            private set
            {
                userPhoto = value;
                NotifyPropertyChanged("UserPhoto");

            }
        }

        private BitmapImage userPhoto1;
        public BitmapImage UserPhoto1
        {
            get
            {
                return userPhoto1;
            }
            private set
            {
                userPhoto1 = value;
                NotifyPropertyChanged("UserPhoto1");

            }
        }

        private BitmapImage userPhoto2;
        public BitmapImage UserPhoto2
        {
            get
            {
                return userPhoto2;
            }
            private set
            {
                userPhoto2 = value;
                NotifyPropertyChanged("UserPhoto2");

            }
        }

        private BitmapImage userPhoto3;
        public BitmapImage UserPhoto3
        {
            get
            {
                return userPhoto3;
            }
            private set
            {
                userPhoto3 = value;
                NotifyPropertyChanged("UserPhoto3");

            }
        }

        private BitmapImage userPhoto4;
        public BitmapImage UserPhoto4
        {
            get
            {
                return userPhoto4;
            }
            private set
            {
                userPhoto4 = value;
                NotifyPropertyChanged("UserPhoto4");

            }
        }

        public ConversationMessageViewModel(string accessToken, MessageInfo message)
        {
            ChatId = message.ChatId;
            Title = message.Title;
            Body = message.Body;
            Date = message.Date;

            var messages = new UsersRequestsLibrary(accessToken);

            var chatActiveUsers = message.ChatActiveUsers;
            if (!String.IsNullOrEmpty(chatActiveUsers))
            {
                var getActiveUsersCommand = messages.CreateGetUsersCommand(message.ChatActiveUsers, new string[] { UserInfo.FieldPhotoLink });
                getActiveUsersCommand.ExecuteCompleted += (sender, e) =>
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
                    for (int i = 0; i < e.Result.Count; i++)
                    {
                        LoadPhoto(e.Result[i].PhotoLink, i + 1);
                    }
                };
                getActiveUsersCommand.Execute(true);
            }

            var getUsersCommand = messages.CreateGetUsersCommand(message.UserId, new string[] { UserInfo.FieldFirstName, UserInfo.FieldLastName, UserInfo.FieldPhotoLink });
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
                UserId = user.UserId;
                UserName = String.Concat(user.FirstName, Chars.Space, user.LastName);

                LoadPhoto(user.PhotoLink);
            };
            getUsersCommand.Execute(true);
        }
        private void LoadPhoto(string link, int? number = null)
        {
            if (!String.IsNullOrEmpty(link))
            {
                var loadPhotoCommand = new LoadImageCommand(new Uri(link));
                loadPhotoCommand.ExecuteCompleted += (sender, e) =>
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
                    if (number.HasValue)
                    {
                        switch (number.Value)
                        {
                            case 1: UserPhoto1 = e.Result; return;
                            case 2: UserPhoto2 = e.Result; return;
                            case 3: UserPhoto3 = e.Result; return;
                            case 4: UserPhoto4 = e.Result; return;
                        }
                    }
                    UserPhoto = e.Result;
                };
                loadPhotoCommand.Execute(false);
            }
        }
    }
}