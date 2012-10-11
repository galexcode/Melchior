using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Описание приглашения в друзья
    /// <remarks>Author WrMax</remarks>
    ///
    /// </summary>
    public class RequestInfo : DataInfo
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public string UserId { get { return Data.GetFieldTextContent("uid"); } }
        /// <summary>
        /// заголовок сообщения или беседы
        /// </summary>
        public string Message { get { return Data.GetFieldTextContent("message"); } }

        public RequestInfo(VKData data)
            : base(data)
        {
        }
    }
}