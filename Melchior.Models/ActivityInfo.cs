using System;
using Melchior.Data.Common;
using Melchior.Common;

namespace Melchior.Models
{
	/// <summary>
	/// Информация об активности
	/// <remarks>Author WrMax</remarks>
	///
	/// </summary>
    public class ActivityInfo : DataInfo
	{
		/// <summary>
		/// текущий статус пользователя (1 – в сети, 0 – не в сети);
		/// </summary>
		public bool Online
		{
            get
            {
                string content = Data.GetFieldTextContent("online");
                if (String.IsNullOrEmpty(content)) return false;
                return content.Equals(Chars.One);
            }
		}
		/// <summary>
		/// дата последней активности пользователя (в формате unixtime).
		/// </summary>
        public string Time { get { return Data.GetFieldTextContent("time"); } } 

		public ActivityInfo(VKData data) : base(data)
		{
		}
	}
}