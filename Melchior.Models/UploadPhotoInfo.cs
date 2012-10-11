using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    public class UploadPhotoInfo : DataInfo
    {
        public string Server { get { return Data.GetFieldTextContent("server"); } }

        public string Photo { get { return Data.GetFieldTextContent("photo"); } }

        public string Hash { get { return Data.GetFieldTextContent("hash"); } }

        public UploadPhotoInfo(VKData data)
            : base(data)
        {
        }
    }
}
