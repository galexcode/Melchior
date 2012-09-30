using System;

namespace Melchior.Data.Common
{
	public abstract class VKData
	{
		public abstract string GetName();
		
		public abstract string GetTextContent();
		
		public abstract VKDataCollection GetChildren();

        public abstract VKDataCollection GetChildren(string tagName);

        public abstract VKData GetField(string tagName);

		public string GetFieldTextContent(string tagName)
		{
            if (tagName == null) throw new ArgumentNullException("tagName");

			VKData data = GetField(tagName);
            if (data == null) return String.Empty;

            return data.GetTextContent();
		}
	}
}