using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Коллекция сообщений
    /// <remarks>Author WrMax</remarks>
    /// </summary>	
    public class MessageCollection : List<MessageInfo>
    {
        public MessageCollection(VKData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            VKDataCollection items = data.GetChildren("message");
            for (int i = 0, length = items.GetLength(); i < length; i++)
            {
                Insert(0, new MessageInfo(items.GetItem(i)));
            }
        }
    }
}