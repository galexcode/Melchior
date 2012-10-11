using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// 
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class DocumentInfo : AttachmentInfo
    {
        /// <summary>
        /// идентификатор документа 
        /// </summary>
        public string DocumentId { get { return Data.GetFieldTextContent("did"); } }
        /// <summary>
        /// идентификатор владельца документа
        /// </summary>
        public string OwnerId { get { return Data.GetFieldTextContent("owner_id"); } }
        /// <summary>
        /// название документа 
        /// </summary>
        public string Title { get { return Data.GetFieldTextContent("title"); } }
        /// <summary>
        /// размер документа 
        /// </summary>
        public string Size { get { return Data.GetFieldTextContent("size"); } }
        /// <summary>
        /// формат документа (расширение файла) 
        /// </summary>
        public string Extension { get { return Data.GetFieldTextContent("ext"); } }
        /// <summary>
        /// адрес для загрузки документа 
        /// </summary>
        public string Url { get { return Data.GetFieldTextContent("url"); } }

        public string Thumb { get { return Data.GetFieldTextContent("thumb"); } }

        public string AccessKey { get { return Data.GetFieldTextContent("access_key"); } }

        public DocumentInfo(VKData data)
            : base(data)
        {
        }
    }
}