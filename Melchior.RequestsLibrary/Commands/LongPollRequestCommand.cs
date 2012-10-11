using System;
using Melchior.Request.Common;
using Melchior.Models;
using System.Xml.Linq;
using Melchior.Request.Xml;
using System.Runtime.Serialization;
using Melchior.Common;
using Melchior.Data.Common;
using Newtonsoft.Json.Linq;

namespace Melchior.RequestsLibrary
{
    public class LongPollRequestCommand : BaseVKRequestCommand<LongPollServerAnswerInfo>
    {
        public LongPollRequestCommand(string server, string key)
        {
            Query = String.Format("http://{0}?act=a_check&key={1}", server, key);
        }

        protected override string GetStorageFileName()
        {
            throw new NotSupportedException();
        }

        protected override LongPollServerAnswerInfo ConvertResult(string result)
        {
            var document = JObject.Parse(result);
            return new LongPollServerAnswerInfo(new VKDataJSON(document));
        }
    }
}