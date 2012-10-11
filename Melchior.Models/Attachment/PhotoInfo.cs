using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// 
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class PhotoInfo : AttachmentInfo
    {
        /// <summary>
        /// идентификатор фотографии 
        /// </summary>
        public string PictureId { get { return Data.GetFieldTextContent("pid"); } }
        /// <summary>
        /// идентификатор владельца фотографии 
        /// </summary>
        public string OwnerId { get { return Data.GetFieldTextContent("owner_id"); } }
        /// <summary>
        /// адрес изображения 
        /// </summary>
        public string Source { get { return Data.GetFieldTextContent("src"); } }
        /// <summary>
        /// адрес крупной версии 
        /// </summary>
        public string SourceBig { get { return Data.GetFieldTextContent("src_big"); } }

        public string SourceSmall { get { return Data.GetFieldTextContent("src_small"); } }
        public string Width { get { return Data.GetFieldTextContent("width"); } }
        public string Height { get { return Data.GetFieldTextContent("height"); } }
        public string Text { get { return Data.GetFieldTextContent("text"); } }
        public string Created { get { return Data.GetFieldTextContent("created"); } }
        public string AccessKey { get { return Data.GetFieldTextContent("access_key"); } }

        public PhotoInfo(VKData data)
            : base(data)
        {
        }
    }
}