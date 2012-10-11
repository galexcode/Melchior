using System;
using Melchior.Models;
using System.Windows.Media.Imaging;
using Melchior.Common;

namespace Melchior.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name { get; private set; }
        public string PhotoBigLink { get; private set; }

        public ProfileViewModel(UserInfo user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Name = String.Concat(user.FirstName, Chars.Space, user.LastName);
            PhotoBigLink = user.PhotoBigLink;
        }
    }
}