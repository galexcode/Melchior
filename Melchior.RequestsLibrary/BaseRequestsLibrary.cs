using System;
using Melchior.Request.Common;
using System.Xml.Linq;
using Melchior.Request.Xml;
using Melchior.Common;
using Melchior.Models;
using Melchior.Data.Common;

namespace Melchior.RequestsLibrary
{
    /// <summary>
    /// Базовый класс библиотеки методов 
    /// author wrmax
    /// </summary>
    public abstract class BaseRequestsLibrary
    {
        public readonly string AccessToken;
        public BaseRequestsLibrary(string accessToken)
        {
            AccessToken = accessToken;
        }

        internal VKRequestCommand<bool> CreateBooleanRequestCommand(string methodName)
        {
            return new VKRequestCommand<bool>(AccessToken, methodName, (data) => { return data.GetTextContent().Equals(Chars.One); });
        }

        internal VKRequestCommand<string> CreateStringRequestCommand(string methodName)
        {
            return new VKRequestCommand<string>(AccessToken, methodName, (data) => { return data.GetTextContent(); });
        }

        internal VKRequestCommand<UserCollection> CreateUserCollectionRequestCommand(string methodName)
        {
            return new VKRequestCommand<UserCollection>(AccessToken, methodName, (data) => { return new UserCollection(data); });
        }

        internal VKRequestCommand<UploadInfo> CreateUploadInfoRequestCommand(string methodName)
        {
            return new VKRequestCommand<UploadInfo>(AccessToken, methodName, (data) => { return new UploadInfo(data); });
        }

        /// <summary>
        /// Универсальный метод, позволяющий выполнять произвольный набор методов API одним запросом.
        /// </summary>
        internal void Execute()
        {
            throw new NotSupportedException();
        }
    }
}