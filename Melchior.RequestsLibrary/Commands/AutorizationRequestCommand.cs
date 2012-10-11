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
    public class AutorizationRequestCommand : BaseVKRequestCommand<AuthorizationInfo>
    {
        private const string QueryHeader = "https://oauth.vk.com/token";

        private const string ClientId = "2979806";
        private const string ClientSecret = "LdW65MFyWqq8C4nLFkjK";

        public AutorizationRequestCommand(string userName, string password, string scope)
        {
            Query = String.Concat(QueryHeader, "?grant_type=password&client_id=", ClientId, "&client_secret=", ClientSecret, "&username=", userName, "&password=", password, "&scope=", scope);
        }

        public AutorizationRequestCommand(string userName, string password)
            : this(userName, password, "messages,offline,friends,photos")
        {
        }

        protected override string GetStorageFileName()
        {
            throw new NotSupportedException();
        }

        protected override AuthorizationInfo ConvertResult(string result)
        {
            var document = JObject.Parse(result);
            return new AuthorizationInfo(new VKDataJSON(document));
        }
    }
}