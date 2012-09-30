using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
	/// <summary>
	/// Родственная связь
	/// <remarks>Author WrMax</remarks>
	///
	/// </summary>
	public class RelativeInfo : DataInfo
	{
		/// <summary>
		/// Id пользователя
		/// </summary>
		public string UserId { get { return  Data.GetFieldTextContent("uid"); } }
		/// <summary>
		/// type может принимать одно из следующих значений: grandchild, grandparent, child, sibling, parent. 
		/// </summary>
		public string Type { get { return  Data.GetFieldTextContent("type"); } }

		public RelativeInfo(VKData data) : base(data)
		{
		}
	}
}