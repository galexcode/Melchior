using System;
using System.Collections.Generic;
using Melchior.Models;
using Melchior.RequestsLibrary;
using Melchior.Common;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Melchior.ViewModels.AttachmentsViewModel;
using System.Linq;
namespace Melchior.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        public string OwnerUserId { get; private set; }
        public string UserId { get; private set; }
        public string ChatId { get; private set; }

        public string MessageId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string Date { get; private set; }

        public List<ForwardMessageViewModel> ForwardMessages { get; private set; }
        public List<AttachmentViewModel> Attachments { get; private set; }
        public GeoInfo Geo { get; private set; }
        
        public MessageViewModel(string accessToken, string ownerUserId, string chatId, string userId, MessageInfo message)
        {
            OwnerUserId = ownerUserId;
            ChatId = chatId;
            MessageId = message.MessageId;
            UserId = message.UserId;
            Title = message.Title;
            Body = message.Body;
            Date = message.Date;
            
            var forwardMessages = message.ForwardMessages;
            if (forwardMessages != null)
            {
                ForwardMessages = forwardMessages.Select(x => new ForwardMessageViewModel(accessToken, x)).ToList();
            }
            Geo = message.Geo;

            Attachments = new List<AttachmentViewModel>();
            var attachments = message.Attachments;
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var type = attachment.GetType();
                    if (type == typeof(PhotoInfo))
                    {
                        Attachments.Add(new PhotoViewModel((PhotoInfo)(attachment)));
                    }
                    if (type == typeof(AudioInfo))
                    {
                        Attachments.Add(new AudioViewModel((AudioInfo)(attachment)));
                    }
                    if (type == typeof(DocumentInfo))
                    {
                        Attachments.Add(new DocumentViewModel((DocumentInfo)(attachment)));
                    }
                    if (type == typeof(VideoInfo))
                    {
                        Attachments.Add(new VideoViewModel((VideoInfo)(attachment)));
                    }
                }
            }
        }
    }
}
