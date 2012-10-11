using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Информация от LongPoll-сервера
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class LongPollInfo : DataInfo
    {
        /// <summary>
        /// секретный ключ сессии
        /// </summary>
        public string Key { get { return Data.GetFieldTextContent("key"); } }
        /// <summary>
        /// адрес сервера
        /// </summary>
        public string Server { get { return Data.GetFieldTextContent("server"); } }
        /// <summary>
        /// номер последнего события
        /// </summary>
        public string TS { get { return Data.GetFieldTextContent("ts"); } }

        public LongPollInfo(VKData data)
            : base(data)
        {
        }
    }
}