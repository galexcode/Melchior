using System;

namespace Melchior.Request.Common
{
	/// <summary>
		/// Thrown when there were problems contacting the remote API server, either
		/// because of a network error, or the server returned a bad status code.
		/// </summary>
	
	public class ApiException : Exception 
    {

		public ApiException(string detailMessage) : base(detailMessage)
        {
		}
	}
}
