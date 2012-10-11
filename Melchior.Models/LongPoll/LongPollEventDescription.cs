using System;
using Melchior.Data.Common;
using System.Collections.Generic;

namespace Melchior.Models
{
    public class LongPollEventDescription : List<string>
    {
        public LongPollEventDescription(VKData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            VKDataCollection items = data.GetChildren();
            for (int i = 0, length = items.GetLength(); i < length; i++)
            {
                Add(items.GetItem(i).GetTextContent());
            }
        }
    }
}