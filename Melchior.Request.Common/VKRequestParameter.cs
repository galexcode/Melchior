using System;

namespace Melchior.Request.Common
{
	/// <summary>
	/// Параметр команды VKRequestCommand
	/// @author wrmax
	///
	/// </summary>
	public class VKRequestParameter
	{
        public readonly string Name;
        public object Value { get; set; }

		public VKRequestParameter(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}
}