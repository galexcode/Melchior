using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Коллекция данных по пользователям
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class UserCollection : List<UserInfo>
    {
        public UserCollection(VKData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            VKDataCollection items = data.GetChildren("user");
            for (int i = 0, length = items.GetLength(); i < length; i++)
            {
                Add(new UserInfo(items.GetItem(i)));
            }
        }
    }
}