using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Melchior.ViewModels;

namespace Melchior
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultMessageTemplate
        {
            get;
            set;
        }

        public DataTemplate MyMessageTemplate
        {
            get;
            set;
        }

        public DataTemplate FriendMessageTemplate
        {
            get;
            set;
        }

        public DataTemplate ChatMyMessageTemplate
        {
            get;
            set;
        }

        public DataTemplate ChatFriendMessageTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var message = item as MessageViewModel;
            if (message == null) return DefaultMessageTemplate;

            if (!String.IsNullOrEmpty(message.ChatId))
            {
                if (message.OwnerUserId == message.UserId)
                {
                    return ChatMyMessageTemplate;
                }
                else
                {
                    return ChatFriendMessageTemplate;
                }
            }

            if (message.OwnerUserId == message.UserId)
            {
                return MyMessageTemplate;
            }
            else
            {
                return FriendMessageTemplate;
            }
        }
    }
}
