using System;
using System.Collections.Generic;
using Melchior.Data.Common;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Melchior.Request.Xml
{
    public class VKDataCollectionJSON : VKDataCollection
    {
        protected readonly JArray DataList;
        protected readonly int Length;

        public VKDataCollectionJSON(JArray dataList)
        {
            if (dataList == null) throw new ArgumentNullException("dataList");

            DataList = dataList;
            Length = dataList.Count;
        }

        public override VKData GetItem(int index)
        {
            return new VKDataJSON(DataList[index]);
        }

        public override int GetLength()
        {
            return Length;
        }
    }
}