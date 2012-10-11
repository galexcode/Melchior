using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Информация о диалогах
    /// <remarks>Author WrMax</remarks>
    ///
    /// </summary>
    public class DialogsDataInfo
    {
        private List<UserInfo> profiles;
        public List<UserInfo> Profiles
        {
            get
            {
                return profiles;
            }
        }
        public List<ChatInfo> chatList;
        public List<ChatInfo> ChatList
        {
            get
            {
                return chatList;
            }
        }

        public DialogsDataInfo(VKData data)
        {
            if (data == null) throw new ArgumentNullException("data");

            profiles = new List<UserInfo>();
            chatList = new List<ChatInfo>();

            VKDataCollection items = data.GetChildren("item");
            for (int i = 0, length = items.GetLength(); i < length; i++)
            {
                VKData childItem = items.GetItem(i);

                VKDataCollection dataTypes = childItem.GetChildren("type");
                VKData dataType = dataTypes.GetItem(0);
                if (dataType != null)
                {
                    string itemType = dataType.GetTextContent();
                    if (itemType.Equals("profile"))
                    {
                        //profiles.Add(new UserInfo(xmlItem));
                    }
                    else if (itemType.Equals("chat"))
                    {
                        //chatList.Add(new ChatInfo(xmlItem));
                    }
                }
            }
        }
    }
}