using System;
using System.Collections.Generic;
using Melchior.Data.Common;

namespace Melchior.Models
{
	/// <summary>
	/// Коллекция фотографий
	/// <remarks>Author WrMax</remarks>
	///
	/// </summary>
	
	public class PhotoCollection : List<PhotoInfo>
	{
		public PhotoCollection(VKData data)
		{
			if (data == null)
			{
                throw new ArgumentNullException("data");
			}
			
			VKDataCollection items = data.GetChildren("photo");
			for(int i = 0, length = items.GetLength(); i < length; i++)
			{
				Add(new PhotoInfo(items.GetItem(i)));
			}
		}
	}
}