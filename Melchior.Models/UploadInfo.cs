using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Информация для загрузки данных
    /// </summary>
    public class UploadInfo : DataInfo
    {
        /// <summary>
        /// Адрес сервера для загрузки данных
        /// </summary>
        public string UploadUrl { get { return Data.GetFieldTextContent("upload_url"); } }

        public UploadInfo(VKData data)
            : base(data)
        {
        }
    }
}
