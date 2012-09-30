using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using Melchior.Data.Common;
using Melchior.Request.Common;
using Melchior.Common;
using System.Xml.Linq;
using System.ComponentModel;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Melchior.Request.Xml
{
    public class VKRequestCommand<TResult> : BaseVKRequestCommand<TResult>
    {
        private const string QueryHeader = "https://api.vk.com/method/";
        public readonly string MethodName;
        public readonly VKRequestTypes RequestType;

        public Func<VKData, TResult> Converter;

        public VKRequestCommand(string accessToken, string methodName, Func<VKData, TResult> converter, VKRequestTypes requestType = VKRequestTypes.XML) 
        {
            MethodName = methodName;
            Converter = converter;
            RequestType = requestType;
            Query = String.Concat(QueryHeader, methodName, requestType == VKRequestTypes.XML ? ".xml" : String.Empty, "?access_token=", accessToken);
        }
        public VKRequestCommand(string clientId, string clientSecret, string methodName, Func<VKData, TResult> converter, VKRequestTypes requestType = VKRequestTypes.XML)
        {
            MethodName = methodName;
            Converter = converter;
            RequestType = requestType;
            Query = String.Concat(QueryHeader, methodName, requestType == VKRequestTypes.XML ? ".xml" : String.Empty, "?client_id=", clientId, "&client_secret=", clientSecret);
        }

        protected override string GetStorageFileName()
        {
            var builder = new StringBuilder();
            builder.Append(MethodName);
            builder.Append(Chars.Underline);
            foreach (var parameter in Parameters)
            {
                builder.Append(Chars.Underline);
                builder.Append(parameter.Name);
                builder.Append(Chars.Defis);
                builder.Append(parameter.Value);
            }
            builder.Append(".xml");
            return builder.ToString();
        }

        protected sealed override TResult ConvertResult(string result)
        {
            VKData data;
            if (RequestType == VKRequestTypes.XML)
            {
                var document = XDocument.Parse(result);
                data = new VKDataXML(document.Root);
                if (data.GetName() == "error")
                {
                    throw new VKRequestCustomException(data);
                }
            }
            else
            {
                var document = JObject.Parse(result);
                if (document["error"] != null)
                {
                    throw new VKRequestCustomException(new VKDataJSON(document["error"]));
                }
                data = new VKDataJSON(document["response"]);
                
            }
            return Converter(data);
        }
    }
}
