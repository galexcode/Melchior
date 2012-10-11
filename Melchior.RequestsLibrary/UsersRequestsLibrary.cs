using System;
using Melchior.Models;
using Melchior.Data.Common;
using Melchior.Common;
using Melchior.Request.Common;
using Melchior.Request.Xml;
using System.Xml.Linq;

namespace Melchior.RequestsLibrary
{
    /// <summary>
    /// Работа с пользователями
    /// author wrmax
    /// </summary>
    public class UsersRequestsLibrary : BaseRequestsLibrary
    {
        public UsersRequestsLibrary(string accessToken)
            : base(accessToken)
        {
        }
        /// <summary>
        /// Возвращает информацию о пользователях по их ID
        /// </summary>
        /// <param name="userIds">Перечисленные через запятую ID пользователей или их короткие имена (screen_name). Максимум 1000 пользователей.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Доступные значения: uid, first_name, last_name, nickname, screen_name, sex, bdate (birthdate), city, country, timezone, photo, photo_medium, photo_big, has_mobile, rate, contacts, education, online, counters.</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom.</param>
        public VKRequestCommand<UserCollection> CreateGetUsersCommand(string userIds, string[] fields, string nameCase)
        {
            if (userIds == null || String.IsNullOrEmpty(userIds))
            {
                throw new ArgumentNullException("userIds");
            }
            const string methodName = "users.get";
            var command = CreateUserCollectionRequestCommand(methodName);
            if (userIds != null && !String.IsNullOrEmpty(userIds)) command.Parameters.Add("uids", userIds);
            if (fields != null && fields.Length > 0) command.Parameters.Add("fields", StringUtils.Join(fields, Chars.Comma));
            if (nameCase != null && !String.IsNullOrEmpty(nameCase)) command.Parameters.Add("name_case", nameCase);
            return command;
        }
        public VKRequestCommand<UserCollection> CreateGetUsersCommand(string userIds, string[] fields)
        {
            return CreateGetUsersCommand(userIds, fields, null);
        }
    }
}