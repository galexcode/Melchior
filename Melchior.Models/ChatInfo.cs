using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Информация о чате
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class ChatInfo : DataInfo
    {
        /// <summary>
        /// ID чата
        /// </summary>
        public string ChatId { get { return Data.GetFieldTextContent("chat_id"); } }
        /// <summary>
        /// Заголовок чата
        /// </summary>
        public string Title { get { return Data.GetFieldTextContent("title"); } }
        /// <summary>
        /// Идентификаторы пользователей
        /// TODO дописать
        /// </summary>
        public List<string> UserIds
        {
            get
            {
                var usersField = Data.GetField("users");
                if (usersField == null) return null;

                var userIds = new List<string>();
                VKDataCollection items = usersField.GetChildren();
                for (int i = 0, length = items.GetLength(); i < length; i++)
                {
                    VKData xmlItem = items.GetItem(i);
                    userIds.Add(xmlItem.GetTextContent());
                }
                return userIds;
            }
        }

        public ChatInfo(VKData data)
            : base(data)
        {
        }
    }
}