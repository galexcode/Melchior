using System;
using System.Linq;
using System.Collections.Generic;
using Melchior.Data.Common;
using System.Xml.Linq;

namespace Melchior.Request.Xml
{
    public class VKDataXML : VKData
    {
        protected readonly XElement Root;

        public VKDataXML(XElement rootElement)
        {
            if (rootElement == null) throw new ArgumentNullException("rootElement");

            Root = rootElement;
        }

        public override string GetName()
        {
            return Root.Name.LocalName;
        }

        public override string GetTextContent()
        {
            return Root.Value;
        }

        public override VKData GetField(string tagName)
        {
            if (tagName == null) throw new ArgumentNullException("tagName");

            var collection = GetChildren(tagName);
            if (collection == null) return null;
            if (collection.GetLength() < 1) return null;

            return collection.GetItem(0);
        }
        public override VKDataCollection GetChildren()
        {
            return new VKDataCollectionXML(Root.Elements().ToList());
        }
        public override VKDataCollection GetChildren(string tagName)
        {
            return new VKDataCollectionXML(Root.Elements(tagName).ToList());
        }
    }
}