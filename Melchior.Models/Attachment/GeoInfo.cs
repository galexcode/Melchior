using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    
    public struct CoordinatesDescription
    {
        public string X;
        public string Y;
    }

    /// <summary>
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class PlaceInfo : DataInfo
    {
        public string Title { get { return Data.GetFieldTextContent("Title"); } }
        public string Country { get { return Data.GetFieldTextContent("Country"); } }
        public string City { get { return Data.GetFieldTextContent("City"); } }

        public PlaceInfo(VKData data) : base(data)
        {
            
        }
    }
    

	/// <summary>
	/// 
	/// <remarks>Author WrMax</remarks>
	/// </summary>
	public class GeoInfo : DataInfo
	{
        public string Type { get { return Data.GetFieldTextContent("Type"); } }
        public CoordinatesDescription Coordinates
        {
            get
            {
                var content = Data.GetFieldTextContent("Coordinates");
                if (content == null) return default(CoordinatesDescription);
                return new CoordinatesDescription()
                {
                    X = content.Substring(0, content.IndexOf(' ')),
                    Y = content.Substring(content.IndexOf(' ') + 1)
                };
            } 
        }
        public PlaceInfo Place
        {
            get
            {
                var placeField = Data.GetField("place");
                if (placeField == null) return null;
                return new PlaceInfo(placeField);
            }
        }

        public GeoInfo(VKData data)
            : base(data)
		{
		}
	}
}