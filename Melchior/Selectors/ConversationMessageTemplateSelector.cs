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
    public class ConversationMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserMessageTemplate
        {
            get;
            set;
        }

        public DataTemplate ChatMessageTemplate
        {
            get;
            set;
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var message = item as ConversationMessageViewModel;
            if (message != null)
            {
                if (!String.IsNullOrEmpty(message.ChatId))
                {
                    return ChatMessageTemplate;
                }
                else
                {
                    return UserMessageTemplate;
                }
            }
            return UserMessageTemplate;
        }
    }
}