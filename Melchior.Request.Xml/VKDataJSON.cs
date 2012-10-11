using System;
using System.Linq;
using System.Collections.Generic;
using Melchior.Data.Common;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Melchior.Request.Xml
{
    public class VKDataJSON : VKData
    {
        protected readonly JToken Root;

        public VKDataJSON(JToken rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException("rootElement");

            Root = rootElement;
        }

        public override string GetName()
        {
            return Root.Type.ToString();
        }

        public override VKData GetField(string tagName)
        {
            return new VKDataJSON(Root[tagName]);
        }

        public override string GetTextContent()
        {
            return Root.ToString();
        }

        public override VKDataCollection GetChildren()
        {
            var array = new JArray();
            foreach (var item in Root.Children()) array.Add(item);
            return new VKDataCollectionJSON(array);
        }

        public override VKDataCollection GetChildren(string tagName)
        {
            return GetChildren();
        }
    }
}