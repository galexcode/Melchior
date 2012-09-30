using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    public class LongPollServerAnswerInfo : DataInfo
    {
        /// <summary>
        /// номер последнего события
        /// </summary>
        public string TS { get { return Data.GetFieldTextContent("ts"); } }

        public LongPollEventCollection Updates 
        { 
            get 
            {
                var updatsField = Data.GetField("updates");
                if (updatsField == null) return null;
                return new LongPollEventCollection(updatsField);
            } 
        }

        public LongPollServerAnswerInfo(VKData data) : base(data)
        {
        }
    }
}
