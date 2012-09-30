using System;
using System.Collections.Generic;
using Melchior.Data.Common;
using System.Xml.Linq;

namespace Melchior.Request.Xml
{
	public class VKDataCollectionXML : VKDataCollection
	{
        protected readonly List<XElement> DataList;
		protected readonly int Length;
		
		public VKDataCollectionXML(List<XElement> dataList)
		{
            if (dataList == null) throw new ArgumentNullException("dataList");

			DataList = dataList;
			Length = dataList.Count;
		}

		public override VKData GetItem(int index)
		{
			return new VKDataXML(DataList[index]);
		}

		public override int GetLength()
		{
			return Length;
		}
	}
}
