using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Коллекция приглашений в друзья
    /// <remarks>Author WrMax</remarks>
    /// </summary>	
    public class RequestCollection : List<RequestInfo>
    {
        public RequestCollection(VKData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            VKDataCollection items = data.GetChildren("request");
            for (int i = 0, length = items.GetLength(); i < length; i++)
            {
                Add(new RequestInfo(items.GetItem(i)));
            }
        }
    }
}