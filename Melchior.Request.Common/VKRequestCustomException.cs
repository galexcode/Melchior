using System;
using Melchior.Data.Common;

namespace Melchior.Request.Common
{
	/// <summary>
	/// Описание исключения вызванной командой VKRequestCommand
	/// @author wrmax
	/// </summary>
	
	public class VKRequestCustomException : Exception
	{
		/// <summary>
		/// Код ошибки
		/// </summary>
		public int Code;

		public VKRequestCustomException(VKData dataErrorCode, VKData dataErrorMessage): base("VKRequestCommand Exception: " + dataErrorCode.GetTextContent() + "\n" + dataErrorMessage.GetTextContent())
		{
            Code = Int32.Parse(dataErrorCode.GetTextContent());
		}
		public VKRequestCustomException(VKData data) : this(data.GetField("error_code"), data.GetField("error_msg"))
		{
		}
	}
}