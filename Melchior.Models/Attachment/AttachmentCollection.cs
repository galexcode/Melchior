using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Коллекция медиа-вложений
    /// <remarks>Author WrMax</remarks>
    /// </summary>	
    public class AttachmentCollection : List<AttachmentInfo>
    {
        public AttachmentCollection(VKData data)
        {
            VKDataCollection items = data.GetChildren("attachment");
            for (int i = 0, length = items.GetLength(); i < length; i++)
            {
                VKData item = items.GetItem(i);
                string itemType = item.GetFieldTextContent("type");
                if (itemType != null)
                {
                    if (itemType.Equals("photo"))
                    {
                        Add(new PhotoInfo(item.GetField(itemType)));
                    }
                    if (itemType.Equals("audio"))
                    {
                        Add(new AudioInfo(item.GetField(itemType)));
                    }
                    if (itemType.Equals("video"))
                    {
                        Add(new VideoInfo(item.GetField(itemType)));
                    }
                }
            }
        }
    }
}