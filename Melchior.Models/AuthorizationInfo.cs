using System;
using Melchior.Data.Common;

namespace Melchior.Models
{
    /// <summary>
    /// Авторизационные данные
    /// <remarks>Author WrMax</remarks>
    ///
    /// </summary>
    public class AuthorizationInfo : DataInfo
    {
        public string AccessToken { get { return Data.GetFieldTextContent("access_token"); } }
        public string ExpiresIn { get { return Data.GetFieldTextContent("expires_in"); } }
        public string UserId { get { return Data.GetFieldTextContent("user_id"); } }

        public AuthorizationInfo(VKData data)
            : base(data)
        {
        }
    }
}