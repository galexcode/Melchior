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
	public abstract class BaseAuthRequestsLibrary
	{
		public readonly string ClientId;
        public readonly string ClientSecret;
        public BaseAuthRequestsLibrary(string clientId, string clientSecret)
		{
            ClientId = clientId;
            ClientSecret = clientSecret;
		}

        public VKRequestCommand<bool> CreateBooleanRequestCommand(string methodName)
        {
            return new VKRequestCommand<bool>(ClientId, ClientSecret, methodName, (data) => { return data.GetTextContent().Equals(Chars.One); });
        }

        public VKRequestCommand<string> CreateStringRequestCommand(string methodName)
        {
            return new VKRequestCommand<string>(ClientId, ClientSecret, methodName, (data) => { return data.GetTextContent(); });
        }

		/// <summary>
		/// Универсальный метод, позволяющий выполнять произвольный набор методов API одним запросом.
		/// </summary>
		public void Execute()
		{
			throw new NotSupportedException();
		}
	}
}