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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Melchior.ViewModels.PageViewModels
{
    public class ConversationViewModel : BaseViewModel
    {
        public string ConversationChatId { get; private set; }
        public string ConversationUserId { get; private set; }

        private string eventNote;
        public string EventNote
        {
            get
            {
                return eventNote;
            }
            set
            {
                eventNote = value;
                NotifyPropertyChanged("EventNote");
            }
        }

        private ObservableCollection<MessageViewModel> messages;
        public ObservableCollection<MessageViewModel> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                NotifyPropertyChanged("Messages");
            }
        }

        public ConversationViewModel(string conversationChatId, string conversationUserId)
        {
            ConversationChatId = conversationChatId;
            ConversationUserId = conversationUserId;
        }


    }
}
