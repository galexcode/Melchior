using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    public class LongPollEventCollection : List<LongPollEventDescription>
	{
        public LongPollEventCollection(VKData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			
			VKDataCollection items = data.GetChildren();
			for(int i = 0, length = items.GetLength(); i < length; i++)
			{
                Add(new LongPollEventDescription(items.GetItem(i)));
			}
		}
	}
}
