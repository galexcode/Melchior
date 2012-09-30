using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
	/// <summary>
	/// <remarks>Author WrMax</remarks>
	/// </summary>
	public class DataInfo
	{
		/// <summary>
		/// Данные
		/// </summary>
		protected readonly VKData Data;

        public DataInfo(VKData data)
		{
            if (data == null) throw new ArgumentNullException("data");

			Data = data;
		}
	}
}
