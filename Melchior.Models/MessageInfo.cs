using System;
using Melchior.Data.Common;
using Melchior.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Описание сообщения
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class MessageInfo : DataInfo
    {
        /// <summary>
        /// ID сообщения. Не передаётся для пересланных сообщений.
        /// </summary>
        public string MessageId { get { return Data.GetFieldTextContent("mid"); } }
        /// <summary>
        /// автор сообщения
        /// </summary>
        public string UserId { get { return Data.GetFieldTextContent("uid"); } }
        /// <summary>
        /// дата отправки сообщения
        /// </summary>
        public string Date { get { return Data.GetFieldTextContent("date"); } }
        /// <summary>
        /// статус прочтения сообщения (0 – не прочитано, 1 – прочитано) Не передаётся для пересланных сообщений.
        /// </summary>
        public bool CanReadState
        {
            get
            {
                string content = Data.GetFieldTextContent("read_state");
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
        }
        /// <summary>
        /// тип сообщения (0 – полученное сообщение, 1 – отправленное сообщение). Не передаётся для пересланных сообщений.
        /// </summary>
        public bool CanOutSign
        {
            get
            {
                string content = Data.GetFieldTextContent("out");
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
        }
        /// <summary>
        /// заголовок сообщения или беседы
        /// </summary>
        public string Title { get { return Data.GetFieldTextContent("title"); } }
        /// <summary>
        /// текст сообщения
        /// </summary>
        public string Body
        {
            get
            {
                string content = Data.GetFieldTextContent("body");
                if (String.IsNullOrEmpty(content)) return String.Empty;
                return content.Replace("<br/>", Environment.NewLine);
            }
        }
        /// <summary>
        /// массив медиа-вложений (если есть)
        /// </summary>
        public AttachmentCollection Attachments
        {
            get
            {
                var attachmentsField = Data.GetField("attachments");
                if (attachmentsField == null) return null;
                return new AttachmentCollection(attachmentsField);
            }
        }
        public GeoInfo Geo
        {
            get
            {
                var geoFielld = Data.GetField("geo");
                if (geoFielld == null) return null;
                return new GeoInfo(geoFielld);
            }
        }
        /// <summary>
        /// массив пересланных сообщений (если есть)
        /// </summary>
        public MessageCollection ForwardMessages
        {
            get
            {
                var forwardMessagesField = Data.GetField("fwd_messages");
                if (forwardMessagesField == null) return null;
                return new MessageCollection(forwardMessagesField);
            }
        }
        /// <summary>
        /// (только для групповых бесед) ID беседы 
        /// </summary>
        public string ChatId { get { return Data.GetFieldTextContent("chat_id"); } }
        /// <summary>
        /// (только для групповых бесед) ID последних участников беседы, разделённых запятыми, но не более 6. 
        /// </summary>
        public string ChatActiveUsers { get { return Data.GetFieldTextContent("chat_active"); } }
        /// <summary>
        /// (только для групповых бесед) количество участников в беседе 
        /// </summary>
        public int UsersCount
        {
            get
            {
                string content = Data.GetFieldTextContent("users_count");
                if (String.IsNullOrEmpty(content)) return 0;
                return Int32.Parse(content);
            }
        }
        /// <summary>
        /// (только для групповых бесед) ID создателя беседы 
        /// </summary>
        public string AdminId { get { return Data.GetFieldTextContent("admin_id"); } }

        public MessageInfo(VKData data)
            : base(data)
        {
        }
    }
}