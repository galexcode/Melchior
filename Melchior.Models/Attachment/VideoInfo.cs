using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// 
    /// <remarks>Author WrMax</remarks>
    /// </summary>
    public class VideoInfo : AttachmentInfo
    {
        /// <summary>
        /// идентификатор видеозаписи 
        /// </summary>
        public string VideoId { get { return Data.GetFieldTextContent("vid"); } }
        /// <summary>
        /// идентификатор владельца видеозаписи 
        /// </summary>
        public string OwnerId { get { return Data.GetFieldTextContent("owner_id"); } }
        /// <summary>
        /// название 
        /// </summary>
        public string Title { get { return Data.GetFieldTextContent("title"); } }
        /// <summary>
        /// описание видеозаписи 
        /// </summary>
        public string Description { get { return Data.GetFieldTextContent("description"); } }
        /// <summary>
        /// длительность видеозаписи (в секундах) 
        /// </summary>
        public string Duration { get { return Data.GetFieldTextContent("duration"); } }
        /// <summary>
        /// превью видеозаписи 
        /// </summary>
        public string Image { get { return Data.GetFieldTextContent("image"); } }
        /// <summary>
        /// большое превью видеозаписи 
        /// </summary>
        public string ImageBig { get { return Data.GetFieldTextContent("image_big"); } }
        /// <summary>
        /// маленькое превью видеозаписи 
        /// </summary>
        public string ImageSmall { get { return Data.GetFieldTextContent("image_small"); } }
        /// <summary>
        /// количество просмотров видеозаписи 
        /// </summary>
        public string Views { get { return Data.GetFieldTextContent("views"); } }
        /// <summary>
        /// дата видеозаписи 
        /// </summary>
        public string Date { get { return Data.GetFieldTextContent("date"); } }

        public string ImageXBig { get { return Data.GetFieldTextContent("image_xbig"); } }

        public string AccessKey { get { return Data.GetFieldTextContent("access_key"); } }

        public VideoInfo(VKData data)
            : base(data)
        {
        }
    }
}